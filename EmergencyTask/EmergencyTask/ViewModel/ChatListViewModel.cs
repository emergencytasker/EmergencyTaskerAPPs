using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using Plugin.Net.Socket;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ChatListViewModel : ViewModelBase
    {

        #region BindableProperty Chat
        /// <summary>
        /// Chat de la propiedad bindable
        /// </summary>
        private ChatModel chat;
        public ChatModel Chat
        {
            get { return chat; }
            set { chat = value; OnPropertyChanged(); if (value != null) { OnChatSelected(value); } }
        }

        private async void OnChatSelected(ChatModel chat)
        {
            if (chat == null) return;
            var me = Usuario.GetUserLogin();
            if (me == null) return;
            chat.IsPenddingVisible = false;
            chat.PenddingMessage = 0;
            await Navigation.PushAsync(new ChatPage(chat.Cliente, chat.Trabajador)
            {
                Title = string.Format(AppResource.ChatCon, chat.NameUser)
            });
        }
        #endregion

        #region Notified Property Source
        /// <summary>
        /// Source
        /// </summary>
        private ObservableCollection<ChatModel> source;
        public ObservableCollection<ChatModel> Source
        {
            get { return source; }
            set { source = value; OnPropertyChanged(); }
        }
        #endregion

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            var chats = await Client.Chat.Where(new Chat
            {
                cliente = App.Perfil == API.Enum.Perfil.Client ? usuario.id : 0,
                trabajador = App.Perfil == API.Enum.Perfil.Tasker ? usuario.id : 0
            }) ?? new List<Chat>(0);
            if (chats.Count() == 0) return;
            var usuarios = await GetUsers(chats);
            var models = await GetModels(usuarios);
            SetSource(models.OrderByDescending(m => m.Date));

            MessagingCenter.Instance.Subscribe<App, ChatMessage>(App.Current as App, "Message", (app, message) =>
            {
                var chat = Source?.FirstOrDefault(s => s.Trabajador == message.Trabajador && s.Cliente == message.Cliente);
                if (chat == null) return;
                chat.Messenger = message.Message;
                chat.LastUpdate = message.Date.ToPrettyDate();
                Source.Remove(chat);
                Source.Insert(0, chat);
            });
        }

        /// <summary>
        /// Asigna los datos de la listview | collectionview
        /// </summary>
        /// <param name="models"></param>
        private void SetSource(IEnumerable<ChatModel> models)
        {
            if (models == null) Source = new ObservableCollection<ChatModel>();
            else Source = new ObservableCollection<ChatModel>(models);
        }

        /// <summary>
        /// Devuelve la lista de modelos basados en una lista de usuarios
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ChatModel>> GetModels(IEnumerable<User> usuarios)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return null;
            List<ChatModel> models = new List<ChatModel>(usuarios.Count());
            foreach (var usuario in usuarios)
            {
                int idtrabajador;
                int idcliente;
                if (me.Perfil == API.Enum.Perfil.Client)
                {
                    idcliente = me.id;
                    idtrabajador = usuario.id;
                }
                else
                {
                    idcliente = usuario.id;
                    idtrabajador = me.id;
                }
                var channel = GetChannel(me, usuario);
                var model = await GetLastUpdate(channel);
                model = new ChatModel
                {
                    ImageUser = Client.GetPath(usuario.imagen),
                    LastUpdate = model?.LastUpdate ?? "",
                    NameUser = usuario.nombre,
                    Messenger = model?.Messenger ?? "",
                    PenddingMessage = model?.PenddingMessage ?? 0,
                    Cliente = idcliente,
                    Trabajador = idtrabajador
                };
                models.Add(model);
            }
            return models;
        }

        /// <summary>
        /// Obtiene el ultimo mensaje del chat
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        private async Task<ChatModel> GetLastUpdate(string channel)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return null;

            var date = await Client.GetDate();
            if (date == null)
            {
                Toast(AppResource.SinConfigurarHora);
                return null;
            }
            var now = date.Value;

            var socket = await SocketFactory.Instance.Resolve();
            var message = (await socket.History(channel, 1)).FirstOrDefault();
            if (message == null) return null;
            if (!message.ContainsKey("message") || !message.ContainsKey("userid") || !message.ContainsKey("date")) return null;

            var messagedate = message["date"];
            var fecha = messagedate.FromMySqlDateTimeFormat();
            var userid = int.Parse(message["userid"]);

            int pending = 0;
            if(me.id != userid) pending++;

            string lastupdate;
            if(now.Day == fecha.Day && now.Month == fecha.Month)
            {
                lastupdate = fecha.ToShortTimeString();
            }
            else if((now.Day - 1) == fecha.Day && now.Month == fecha.Month)
            {
                lastupdate = AppResource.Ayer;
            }
            else
            {
                lastupdate = fecha.ToLongDateString();
            }

            return new ChatModel
            {
                Messenger = message["message"],
                LastUpdate = lastupdate,
                PenddingMessage = pending,
                Date = messagedate
            };
        }

        /// <summary>
        /// DEvuelve el canal de un chat
        /// </summary>
        /// <param name="login"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GetChannel(Usuario login, User user)
        {
            var channel = $"Chat-";
            if (login.Perfil == API.Enum.Perfil.Client)
            {
                channel += $"{login.id}-{user.id}";
            }
            else
            {
                channel += $"{user.id}-{login.id}";
            }
            return channel;
        }

        /// <summary>
        /// Devuelve a los usuarios que estan en la lista de los chat
        /// </summary>
        /// <param name="chats"></param>
        /// <returns></returns>
        private async Task<IEnumerable<User>> GetUsers(IEnumerable<Chat> chats)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return new List<User>(0);
            if (chats == null) return new List<User>(0);
            var idusuarios = new List<int>(chats.Count());
            foreach (var item in chats)
            {
                if(me.Perfil == API.Enum.Perfil.Client)
                {
                    idusuarios.Add(item.trabajador);
                }
                else if(me.Perfil == API.Enum.Perfil.Tasker)
                {
                    idusuarios.Add(item.cliente);
                }
            }
            var ids = idusuarios.Distinct().Select(s => (object)s);
            return await Client.User.Get(ids) ?? new List<User>(0);
        }
    }
}
