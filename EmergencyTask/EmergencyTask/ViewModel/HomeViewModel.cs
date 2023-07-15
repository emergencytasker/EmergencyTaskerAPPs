using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using EmergencyTask.Strings;
using Plugin.Net.Socket;
using System.Threading;
using System;
using System.Configuration;
using EmergencyTask.API.Enum;

namespace EmergencyTask.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {

        #region BindableProperty Carousel
        /// <summary>
        /// Carousel de la propiedad bindable
        /// </summary>
        private ObservableCollection<CarouselModel> carousel;
        public ObservableCollection<CarouselModel> Carousel
        {
            get { return carousel; }
            set { carousel = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Carta
        /// <summary>
        /// Carta de la propiedad bindable
        /// </summary>
        private CartaModel carta;
        public CartaModel Carta
        {
            get { return carta; }
            set { carta = value; OnPropertyChanged(); if (value != null) { value.Action?.Execute(value); } }
        }
        #endregion

        #region BindableProperty Cartas
        /// <summary>
        /// Carta de la propiedad bindable
        /// </summary>
        private ObservableCollection<CartaModel> cartas;
        public ObservableCollection<CartaModel> Cartas
        {
            get { return cartas; }
            set { cartas = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Search
        /// <summary>
        /// Search
        /// </summary>
        private Command search;
        public Command Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoToService
        /// <summary>
        /// GoToService
        /// </summary>
        private ExtendCommand gotoservice;
        public ExtendCommand GoToService
        {
            get { return gotoservice; }
            set { gotoservice = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property InService
        /// <summary>
        /// InService
        private bool inservice;
        public bool InService
        {
            get { return inservice; }
            set { inservice = value; OnPropertyChanged(); }
        }
        #endregion

        public Requestservice CurrentService { get; set; }
        public ISocket SocketIO { get; set; }
        public string ConfirmServiceChannel
        {
            get
            {
                var usuario = Usuario.GetUserLogin();
                if (usuario == null)
                {
                    return "";
                }
                return $"ConfirmService-{usuario.id}";
            }
        }

        public IEnumerable<API.Response.Service> Services { get; private set; }

        #region Notified Property HasPendingItems
        /// <summary>
        /// HasPendingItems
        /// </summary>
        private bool haspendingitems;
        public bool HasPendingPayments
        {
            get { return haspendingitems; }
            set { haspendingitems = value; OnPropertyChanged(); }
        }
        #endregion

        public int PendingPaymentForRequestServiceId { get; set; }

        public HomeViewModel()
        {
            Carousel = new ObservableCollection<CarouselModel>();
            Cartas = new ObservableCollection<CartaModel>();
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;
            Search = new Command(Search_Command);
            GoToService = new ExtendCommand(GoToService_Clicked, new UserValidator());
            Carousel.Add(new CarouselModel
            {
                Wallpaper = "https://cdn.pixabay.com/photo/2015/12/07/10/58/building-1080592_960_720.jpg",
            });
            Carousel.Add(new CarouselModel
            {
                Wallpaper = "https://cdn.pixabay.com/photo/2017/11/29/11/03/ecology-2985781_960_720.jpg",
            });
            Carousel.Add(new CarouselModel
            {
                Wallpaper = "https://cdn.pixabay.com/photo/2017/06/28/04/29/board-2449726_960_720.jpg",
            });
            await LoadCategories();
            await SearchService();
            await ShowPendingToRateService();
            HasPendingPayments = await ShowPendingPayments();
            if (HasPendingPayments)
                await PendingPaymentPopup();
            ConnectToSocket();
            IsBusy = false;
        }

        private async Task<bool> ShowPendingPayments()
        {
            await Task.Delay(1);
            var me = Usuario.GetUserLogin();
            if (me == null) return false;
            var response = await Client.Hito.Where(h => h.cliente, h => h.estado)
                .In(new object[] { me.id }, new object[] { (int)HitoStatus.Created })
                .Execute();
            if (!response.HasExecute) return false;
            var hito = response.Result.FirstOrDefault();
            if (hito == null) return false;
            PendingPaymentForRequestServiceId = hito.idsolicitudservicio;
            return true;
        }

        private async void ConnectToSocket()
        {
            if (string.IsNullOrEmpty(ConfirmServiceChannel))
            {
                return;
            }
            SocketIO = await SocketFactory.Instance.Resolve();
            await SocketIO.Subscribe(ConfirmServiceChannel);
            SocketIO.ConnectionStatus += SocketIO_ConnectionStatus;
            SocketIO.MessageReceived += SocketIO_MessageReceived;
        }

        private void SocketIO_ConnectionStatus(object sender, bool e)
        {
            if(!e)
                Toast(AppResource.OffLine);
        }

        private async void SocketIO_MessageReceived(object sender, Message e)
        {
            System.Diagnostics.Debug.WriteLine(e.RawText, e.Channel);
            Dictionary<string, string> data = null;
            try
            {
                data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Text);
            }
            catch { }
            if (data == null) return;
            if (!data.ContainsKey("idserviciosolicitado")) return;
            await GetNotifications();
        }

        private async void GoToService_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            IsBusy = true;
            await OpenCurrentService();
            IsBusy = false;
        }

        private async Task OpenCurrentService()
        {
            if (CurrentService == null) return;
            Toast(AppResource.EncontramosServicio);
            await GoToServiceDetailPage(CurrentService.id);
        }

        private async Task<bool> SearchService()
        {
            var date = await Client.GetDate();
            if (date == null)
            {
                Toast(AppResource.SinConfigurarHora);
                return false;
            }
            var now = date.Value;
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return false;
            CurrentService = await Client.GetCurrentService(usuario.Perfil, usuario.id);
            InService = CurrentService != null;
            if (!InService) return false;
            return true;
        }

        private async Task LoadCategories()
        {
            var lenguaje = Usuario.GetUserLogin()?.lenguaje ?? Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Services = await Client.GetServices(lenguaje);
            if (Services == null) return;
            var categories = Services.GroupBy(s => s.idcategoria).Select(s => 
            {
                var category = s.ToList().FirstOrDefault();
                return new CartaModel
                {
                    Id = category.idcategoria,
                    Action = new ExtendCommand(Action_Clicked, new InternetValidator()),
                    Image = Client.Path(category.imagencategoria),
                    Title = category.categoria
                };
            });
            Cartas = new ObservableCollection<CartaModel>(categories);
        }

        private async void Search_Command(object obj)
        {
            IsBusy = true;
            await Navigation.PushAsync(new SearchServicePage());
            IsBusy = false;
        }

        private async Task PendingPaymentPopup()
        {
            if (!await Confirm(AppResource.ConfirmGoToPendingPayment))
                return;
            SetDetailPage(new HitoListPage
            {
                BindingContext = new HitoListViewModel(PendingPaymentForRequestServiceId)
            });
        }

        private async void Action_Clicked(object obj, IExecuteValidator[] validators)
        {
            if (Carta == null) return;
            IsBusy = true;
            await Navigation.PushAsync(new CategoriaPage
            {
                BindingContext = new CategoriaViewModel
                {
                    IdCategory = Carta.Id,
                    Category = carta.Title,
                    Services = Services
                }
            });
            Carta = null;
            IsBusy = false;
        }
    }
}