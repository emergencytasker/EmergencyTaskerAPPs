using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using EmergencyTask.API.ER;
using EmergencyTask.Control;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Plugin.Net.Socket;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        #region Notified Property Message
        /// <summary>
        /// Message
        /// </summary>
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsBusy
        /// <summary>
        /// IsBusy
        /// </summary>
        private bool isbusy;
        public new bool IsBusy
        {
            get { return isbusy; }
            set { isbusy = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property SendMessage
        /// <summary>
        /// SendMessage
        /// </summary>
        private ExtendCommand sendmessage;
        public ExtendCommand SendMessage
        {
            get { return sendmessage; }
            set { sendmessage = value; OnPropertyChanged(); }
        }
        #endregion

        public string ChatChannel { get; set; }
        public string SocketChannel { get; set; }
        private ISocket Client { get; set; }
        private ISocket Socket { get; set; }
        public int IdCliente { get; }
        public int IdTrabajador { get; }
        public User Tasker { get; set; }

        public ChatPage(int idcliente, int idtrabajador)
        {
            InitializeComponent();
            IdCliente = idcliente;
            IdTrabajador = idtrabajador;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;

            var query = new Chat
            {
                cliente = IdCliente,
                trabajador = IdTrabajador
            };

            var chat = (await API.Client.Chat.Where(query)).FirstOrDefault();

            if (chat == null) chat = await API.Client.Chat.Add(query);

            if (chat == null)
            {
                IsBusy = false;
                await DisplayAlert(AppResource.Info, AppResource.NoPodemosContinuar, AppResource.Cancelar);
                return;
            }

            if ((Application.Current as App).Perfil == API.Enum.Perfil.Client)
                Tasker = await API.Client.User.Get(IdTrabajador);
            else
                Tasker = await API.Client.User.Get(IdCliente);

            ChatChannel = $"Chat-{IdCliente}-{IdTrabajador}";
            SendMessage = new ExtendCommand(SendMessage_Clicked, new UserValidator(), new InternetValidator());
           
            Client = await SocketFactory.Instance.Resolve();
            await Client.Subscribe(ChatChannel);
            Client.MessageReceived += Socket_MessageReceived;
            Client.ConnectionStatus += Socket_ConnectionStatus;

            SocketChannel = $"Chat-{IdCliente}";
            Socket = await SocketFactory.Instance.Resolve();
            await Socket.Subscribe(SocketChannel);

            await History();

            IsBusy = false;
        }

        private void Socket_ConnectionStatus(object sender, bool e)
        {
            if(!e) UserDialogs.Instance.Toast(AppResource.OffLine);
        }

        private async void SendMessage_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (!arg2.TryGetComparator(out Usuario usuario)) return;
            if (string.IsNullOrEmpty(Message)) return;
            if (Client == null) return;

            var date = DateTime.Now;

            var data = new Dictionary<string, string>
            {
                { "message", Message },
                { "date", date.ToMySqlDateTimeFormat() },
                { "userid", usuario.id.ToString() }
            };

            var status = await Client.Send(ChatChannel, data);
            if (!status)
            {
                return;
            }

            
            var idusuario = usuario.id == IdCliente ? IdTrabajador : IdCliente;
            var view = ProcessMessage(data);
            ScrollTo(view);
            SendNotification(idusuario, data);
            Notify(new ChatMessage
            {
                Message = Message, 
                Date = date, 
                Channel = ChatChannel,
                UserId = idusuario,
                Cliente = IdCliente,
                Trabajador = IdTrabajador
            });

            Message = "";

            if (Socket == null) return;
            await Socket.Send(SocketChannel, data);
        }

        private void Notify(ChatMessage chatMessage)
        {
            if(chatMessage == null) return;
            MessagingCenter.Instance.Send<App, ChatMessage>(App.Current as App, "Message", chatMessage);
        }

        private async void SendNotification(int idusuario, Dictionary<string, string> data)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;
            await API.Client.SendNotification(idusuario, string.Format(AppResource.ChatCon, me.nombre), data["message"], me.id, 14);
        }

        private void AddBubble(BubbleView bubbleView)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
               ChatListView.Children.Add(bubbleView);
            });
        }

        async void Socket_MessageReceived(object sender, Message e)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Socket_MessageReceived] {ex}");
            }
            if (data == null) return;
            var view = ProcessData(data);
            if (view == null) return;
            AddBubble(view);
            await Task.Delay(10);
            Notify(view.Data);
            ScrollTo(view);
        }

        private async Task History()
        {
            if (Client == null) return;
            IsBusy = true;
            var history = await Client?.History(ChatChannel, 100) ?? new List<Dictionary<string, string>>();
            ClearChat();
            history.Reverse();
            foreach (var item in history)
            {
                BubbleView view = ProcessMessage(item);
                if (view == null) continue;
                AddBubble(view);
                ScrollTo(view);
            }
            IsBusy = false;
        }

        private void ClearChat()
        {
            if (ChatListView == null) return;
            ChatListView.Children.Clear();
        }

        void ScrollTo(BubbleView view)
        {
            if (view == null) return;
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Task.Delay(100);
                    await ScrollChatListView.ScrollToAsync(view, ScrollToPosition.End, true);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ScrollTo] ERROR => {ex}");
                }
            });
        }

        private BubbleView ProcessMessage(Dictionary<string, string> data)
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return null;
            if (!data.ContainsKey("message") || !data.ContainsKey("userid")) return null;

            var date = data["date"].FromMySqlDateTimeFormat();

            BubbleView view;

            var chatmessage = new ChatMessage
            {
                Message = data["message"],
                Date = date,
                Channel = ChatChannel,
                UserId = int.Parse(data["userid"]),
                Trabajador = IdTrabajador,
                Cliente = IdCliente
            };

            if (chatmessage.UserId == usuario.id)
            {
                view = new BubbleView(chatmessage, UserType.Me, usuario.nombre, usuario.imagen);
            }
            else
            {
                view = new BubbleView(chatmessage, UserType.Another, Tasker?.nombre ?? "", Tasker == null ? "" : Tasker.imagen);
            }

            return view;
        }

        private BubbleView ProcessData(Dictionary<string, object> data)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach (var item in data)
            {
                items.Add(item.Key, item.Value.ToString());
            }
            return ProcessMessage(items);
        }
    }
}