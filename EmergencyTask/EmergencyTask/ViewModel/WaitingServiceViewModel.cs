using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Essentials;

using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;

using EmergencyTask.Helpers;
using EmergencyTask.Model;

using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using EmergencyTask.Strings;
using Plugin.Net.Socket;
using System.Windows.Input;
using EmergencyTask.ViewModel.Extensions;
using System.Diagnostics;

namespace EmergencyTask.ViewModel
{
    public class WaitingServiceViewModel : ViewModelBase
    {

        /// <summary>
        /// Tiempo por default para las updates del GPS
        /// </summary>
        private int GpsTimeUpdates = 2;

        #region Notified Property Services
        /// <summary>
        /// Services
        /// </summary>
        private ObservableCollection<PendingService> services;
        public ObservableCollection<PendingService> Services
        {
            get { return services; }
            set { services = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoChat
        /// <summary>
        /// GoChat
        /// </summary>
        private Command gochat;
        public Command GoChat
        {
            get { return gochat; }
            set { gochat = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsServicesVisible
        /// <summary>
        /// IsServicesVisible
        /// </summary>
        private bool isservicesvisible;
        public bool IsServicesVisible
        {
            get { return isservicesvisible; }
            set { isservicesvisible = value; OnPropertyChanged(); }
        }

        public User CurrentClient { get; private set; }
        #endregion

        #region Notified Property InService
        /// <summary>
        /// InService
        /// </summary>
        private bool inservice;

        /// <summary>
        /// Servicio actual
        /// </summary>
        public bool InService
        {
            get { return inservice; }
            set { inservice = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Fondos
        /// <summary>
        /// Fondos
        /// </summary>
        private string fondos;
        public string Fondos
        {
            get { return fondos; }
            set { fondos = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoHitos
        /// <summary>
        /// GoHitos
        /// </summary>
        private ExtendCommand gohitos;
        public ExtendCommand GoHitos
        {
            get { return gohitos; }
            set { gohitos = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoAccesories
        /// <summary>
        /// GoAccesories
        /// </summary>
        private ExtendCommand goaccesories;
        public ExtendCommand GoAccesories
        {
            get { return goaccesories; }
            set { goaccesories = value; OnPropertyChanged(); }
        }
        #endregion

        #region Validators
        public IExecuteValidator InternetValidator => new InternetValidator();
        public IExecuteValidator UserValidator => new UserValidator();
        #endregion

        #region Notified Property HasBuyedList
        /// <summary>
        /// HasBuyedList
        /// </summary>
        private bool istoollistcompleted;
        public bool IsToolListCompleted
        {
            get { return istoollistcompleted; }
            set { istoollistcompleted = value; OnPropertyChanged(); if (value) { OnBuyedList(); } }
        }
        #endregion

        #region Notified Property InRouteToAddress
        /// <summary>
        /// InRouteToAddress
        /// </summary>
        private bool inroutetoaddress;
        public bool InRouteToAddress
        {
            get { return inroutetoaddress; }
            set { inroutetoaddress = value; OnPropertyChanged(); if (value) { OnRouteToAddress(); } }
        }
        #endregion

        #region Notified Property TaskerArrivalToAddress
        /// <summary>
        /// InAddress
        /// </summary>
        private bool taskerarrivaltoaddress;
        public bool TaskerArrivalToAddress
        {
            get { return taskerarrivaltoaddress; }
            set { taskerarrivaltoaddress = value; OnPropertyChanged(); if (value) { OnTaskerArrivalToAddress(); } }
        }
        #endregion

        #region Notified Property WasStartedService
        /// <summary>
        /// WasStartedService
        /// </summary>
        private bool wasstartedservice;
        public bool WasStartedService
        {
            get { return wasstartedservice; }
            set { wasstartedservice = value; OnPropertyChanged(); if (value) { TaskerCanConfirmatingService(); } }
        }
        #endregion

        #region Notified Property GoUploadTickets
        /// <summary>
        /// GoUploadTickets
        /// </summary>
        private ExtendCommand gouploadtickets;
        public ExtendCommand GoUploadTickets
        {
            get { return gouploadtickets; }
            set { gouploadtickets = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ArrivalHasBeenConfirmed
        /// <summary>
        /// ArrivalHasBeenConfirmed
        /// </summary>
        private bool arrivalhasbeenconfirmed;
        public bool ArrivalHasBeenConfirmed
        {
            get { return arrivalhasbeenconfirmed; }
            set { arrivalhasbeenconfirmed = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CountForWaitConfirmation
        /// <summary>
        /// CountForWaitConfirmation
        /// </summary>
        private TimeSpan timespan = new TimeSpan(0, 5, 0);
        public TimeSpan CountForWaitConfirmation
        {
            get { return timespan; }
            set { timespan = value; OnPropertyChanged(); }
        }
        #endregion

        #region PendingService
        private PendingService service;
        public PendingService Service
        {
            get { return service; }
            set { service = value; OnPropertyChanged(); if (value != null) { OnServiceSelected(value); } }
        }
        #endregion

        #region BindableProperty Indicaciones
        /// <summary>
        /// Indicaciones de la propiedad bindable
        /// </summary>
        private string indicaciones;
        public string Indicaciones
        {
            get { return indicaciones; }
            set { indicaciones = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Descripcion
        /// <summary>
        /// Descripcion de la propiedad bindable
        /// </summary>
        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property HideCountTimeLabels
        /// <summary>
        /// HideCountTimeLabels
        /// </summary>
        private bool iscounttimelabelvisible = true;
        public bool IsCountTimeLabelVisible
        {
            get { return iscounttimelabelvisible; }
            set { iscounttimelabelvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CloseToTheLocation
        /// <summary>
        /// CloseToTheLocation
        /// </summary>
        private bool closetothelocation;

        public double DistanceToLocation { get; private set; }

        public bool CloseToTheLocation
        {
            get { return closetothelocation; }
            set { closetothelocation = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property startwork
        /// <summary>
        /// GoTimer
        /// </summary>
        private Command startwork;
        public Command StartWork
        {
            get { return startwork; }
            set { startwork = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoInfo
        /// <summary>
        /// GoInfo
        /// </summary>
        private ExtendCommand goinfo;
        public ExtendCommand GoInfo
        {
            get { return goinfo; }
            set { goinfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsTaskListOpened
        /// <summary>
        /// IsTaskListOpened
        /// </summary>
        private bool istasklistopened;
        public bool IsTaskListOpened
        {
            get { return istasklistopened; }
            set { istasklistopened = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property OpenMap
        /// <summary>
        /// OpenMap
        /// </summary>
        private Command openmap;
        public Command OpenMap
        {
            get { return openmap; }
            set { openmap = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property OpenTaskList
        /// <summary>
        /// OpenTaskList
        /// </summary>
        private Command opentasklist;
        public Command OpenTaskList
        {
            get { return opentasklist; }
            set { opentasklist = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsTicketUploadVisible
        /// <summary>
        /// IsTicketUploadVisible
        /// </summary>
        private bool isticketuploadvisible;
        public bool IsTicketUploadVisible
        {
            get { return isticketuploadvisible; }
            set { isticketuploadvisible = value; OnPropertyChanged(); }
        }

        #endregion

        #region BindableProperty BtnMessage
        /// <summary>
        /// BtnMessage de la propiedad bindable
        /// </summary>
        private Command btnmessage;
        public Command BtnMessage
        {
            get { return btnmessage; }
            set { btnmessage = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnCall
        /// <summary>
        /// BtnCall de la propiedad bindable
        /// </summary>
        private Command btncall;
        public Command BtnCall
        {
            get { return btncall; }
            set { btncall = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property UserStatus
        /// <summary>
        /// UserStatus
        /// </summary>
        private bool userstatus;
        public bool UserStatus
        {
            get { return userstatus; }
            set { userstatus = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property OnlineOffline
        /// <summary>
        /// OnlineOffline
        /// </summary>
        private string onlineoffline;
        public string OnlineOffline
        {
            get { return onlineoffline; }
            set { onlineoffline = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property UserStatusChanged
        /// <summary>
        /// UserStatusChanged
        /// </summary>
        private ICommand userstatuschanged;
        public ICommand UserStatusChanged
        {
            get { return userstatuschanged; }
            set { userstatuschanged = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GetGPS
        /// <summary>
        /// GetGPS
        /// </summary>
        private ICommand getgps;
        public ICommand GetGPS
        {
            get { return getgps; }
            set { getgps = value; OnPropertyChanged(); }
        }
        #endregion

        public Xamarin.Forms.GoogleMaps.Map Mapa { get; set; }
        private ISocket SocketIO { get; set; }
        public Requestservice CurrentService { get; set; }
        private string GpsChannel
        {
            get
            {
                if (CurrentService == null) return "GPS";
                else return $"GPS-{CurrentService.id}";
            }
        }
        private string ActivityChannel
        {
            get
            {
                if (CurrentService == null) return "Activity";
                else return $"Activity-{CurrentService.id}";
            }
        }

        public string NewServiceChannel
        {
            get
            {
                var usuario = Usuario.GetUserLogin();
                if (usuario == null) return "NewServiceChannel";
                else return $"NewServiceChannel-{usuario.id}";
            }
        }

        public string ServiceChannel
        {
            get
            {
                return "Service";
            }
        }

        public string GoogleMapsKey { get; set; }

        public User Cliente { get; set; }
        public string Phone { get; set; }

        public WaitingServiceViewModel()
        {

        }

		public WaitingServiceViewModel(Requestservice service)
		{
            CurrentService = service;
		}

		/// <summary>
		/// Asigna el mapa al view model
		/// </summary>
		/// <param name="mapa"></param>
		public void SetMapa(Xamarin.Forms.GoogleMaps.Map mapa)
        {
            Mapa = mapa;
            Mapa.PinClicked -= Mapa_PinClicked;
            Mapa.PinClicked += Mapa_PinClicked;
        }

        private async void Mapa_PinClicked(object sender, PinClickedEventArgs e)
        {
            if (e == null) return;
            if (e.Pin == null) return;
            if (e.Pin.Tag == null) return;
            if (IsBusy) return;
            IsBusy = true;
            int.TryParse(e.Pin.Tag.ToString(), out int idsolicitudservicio);
            if (idsolicitudservicio <= 0) return;
            // show popup
            await Navigation.PushAsync(new DetailServicePage
            {
                BindingContext = new DetailServiceViewModel(idsolicitudservicio)
            });
            IsBusy = false;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            Debug.WriteLine($"[OnAppearing]");
            IsBusy = true;
            OpenMap = new Command(OpenMap_Clicked);
            OpenTaskList = new Command(OpenTaskList_Clicked);
            GoAccesories = new ExtendCommand(GoAccesories_Clicked, InternetValidator);
            GoUploadTickets = new ExtendCommand(GoUploadTickets_Clicked, UserValidator, InternetValidator);
            GoHitos = new ExtendCommand(GoHitos_Clicked, InternetValidator, UserValidator);
            BtnMessage = new Command(BtnMessage_Clicked);
            BtnCall = new Command(BtnCall_Clicked);

            var usuario = Usuario.GetUserLogin();
            if (usuario == null)
            {
                IsBusy = false;
                return;
            }

            UserStatus = usuario.online == 1;
            SetStatusText(UserStatus);
            UserStatusChanged = new Command(UserStatusChanged_Command);

            var tiempogps = await GetVar<int>("tiempogps");
            if(tiempogps > 0)
            {
                GpsTimeUpdates = tiempogps;
            }

            GoogleMapsKey = await GetVar<string>("googlemapskey");

            ConnectToSocket();

            if (Latitud == 0 || Longitud == 0)
            {
                try
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    if (location != null)
                    {
                        Latitud = location.Latitude;
                        Longitud = location.Longitude;
                    }
                }
                catch 
                { 

                }
            }

            if(LocationException != null) Toast(LocationException.Message);

            SetPosition(Latitud, Longitud, usuario.nombre);
            MoveToRegion(new Position(Latitud, Longitud));


            if (await SearchService())
            {
                IsBusy = false;
                return;
            }
            

            await LoadPendingServices();
            await GetHitosMoney();
            await ShowPendingToRateService();
            await SetUserOnline();
            GetGPS = new Command(GetGPS_Command);
            IsBusy = false;
        }

        async Task SetUserOnline()
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;
            await SocketIO?.Online(me.id.ToString());
        }

        private async void GoToInfo_Command(PendingService obj)
        {
            if (obj == null) return;
            IsBusy = true;
            await GoToServiceInfoPage(obj.Id);
            IsBusy = false;
        }

        private async void GetGPS_Command(object obj)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.High,
                Timeout = TimeSpan.FromSeconds(5)
            });
            if (location != null)
            {
                Latitud = location.Latitude;
                Longitud = location.Longitude;
                await Client.User.Update(me.id, new Dictionary<string, string>
                    {
                        { nameof(User.latitud), Latitud.ToString() },
                        { nameof(User.longitud), Longitud.ToString() }
                    });
                Debug.WriteLine($"[POSITION] {Latitud}, {Longitud}");
            }
            if (LocationException != null) Toast(LocationException.Message);
            SetPosition(Latitud, Longitud, me.nombre);
            MoveToRegion(new Position(Latitud, Longitud));
        }

        private async void UserStatusChanged_Command(object arg1)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;
            var newstatus = UserStatus;
            var laststatus = !UserStatus;
            var updated = await Client.SwitchSchedule(me.id, newstatus);
            if (!updated)
            {
                Toast(AppResource.ErrorServer);
                UserStatus = laststatus;
                return;
            }
            UserStatusChanged = null;
            UserStatus = newstatus;
            SetStatusText(UserStatus);
            me.online = newstatus ? 1 : 0;
            Usuario.SetUserLogin(me);
            UserStatusChanged = new Command(UserStatusChanged_Command);
        }

        private void SetStatusText(bool userStatus)
        {
            OnlineOffline = userStatus ? "Online" : "Offline";
        }

        /// <summary>
        /// Programar logica btncall
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnCall_Clicked(object obj)
        {
            IsBusy = true;

            if (Cliente == null)
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    Toast(AppResource.SinInternet);
                    IsBusy = false;
                    return;
                }

                Cliente = await Client.User.Get(CurrentService?.cliente ?? 0);
                if (Cliente == null)
                {
                    Toast(AppResource.NoPodemosContinuar);
                    IsBusy = false;
                    return;
                }

                Phone = Cliente.telefono;
            }

            if (Cliente.telefonoverificado == 0 || string.IsNullOrEmpty(Phone))
            {
                Toast(AppResource.ClienteSinTelefono);
                IsBusy = false;
                return;
            }

            try
            {
                PhoneDialer.Open(Phone);
            }
            catch
            {
                Toast(AppResource.NoPodemosContinuar);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Programar logica btnmessage
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnMessage_Clicked(object obj)
        {
            if (CurrentService == null)
            {
                SetDetailPage(new ChatListPage());
            }
            else
            {
                await Navigation.PushAsync(new ChatPage(CurrentService.cliente, CurrentService.trabajador)
                {
                    Title = CurrentService.categoria + " • " + CurrentService.subcategoria
                });
            }
        }

        private async void OpenMap_Clicked(object obj)
        {
            if (CurrentService == null) return;
            await Xamarin.Essentials.Map.OpenAsync(CurrentService.latitud, CurrentService.longitud);
        }

        /// <summary>
        /// Abre o cierra la lista de tareas por realizar
        /// </summary>
        /// <param name="obj"></param>
        private void OpenTaskList_Clicked(object obj)
        {
            IsTaskListOpened = !IsTaskListOpened;
        }

        /// <summary>
        /// Navega a la vista de accesorios
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void GoAccesories_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            IsBusy = true;
            await GoAccesoriesView();
            IsBusy = false;
        }

        private async Task GoAccesoriesView()
        {
            if (CurrentService == null) return;
            await Navigation.PushAsync(new ToolListPage
            {
                BindingContext = new ToolListViewModel(CurrentService)
            });
        }

        /// <summary>
        /// Navega a la lista de hitos
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void GoHitos_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (CurrentService == null) return;
            IsBusy = true;
            await Navigation.PushAsync(new HitoListPage
            {
                BindingContext = new HitoListViewModel(CurrentService.id)
            });
            IsBusy = false;
        }

        /// <summary>
        /// Se lanza cuando se cambia el item de la coleccion
        /// </summary>
        /// <param name="value"></param>
        private async void OnServiceSelected(PendingService value)
        {
            if (Mapa == null) return;
            var position = new Position(value.Latitud, value.Longitud);
            Device.BeginInvokeOnMainThread(() =>
            {
                Mapa.Pins.Clear();
                var pin = new Pin
                {
                    Position = position,
                    Label = value.Categoria + " • " + value.Subcategoria,
                    Address = value.Direccion,
                    IsDraggable = false,
                    IsVisible = true,
                    Tag = value.Id
                };
                Mapa.Pins.Add(pin);
            });
            MoveToRegion(position);
            await SetRouteToService(Latitud, Longitud, position.Latitude, position.Longitude);
        }

        private void MoveToRegion(Position position)
        {
            if (Mapa == null) return;
            Device.BeginInvokeOnMainThread(() =>
            {
                Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(3)));
            });
        }

        private async Task LoadPendingServices()
        {
            IsBusy = true;
            var usuario = Usuario.GetUserLogin();

            var services = (await Client.Requestservice.Where(new Requestservice
            {
                trabajador = usuario.id,
                idestadoservicio = (int)EstadoServicio.Pendiente
            })).OrderByDescending(r => r.fecha.FromMySqlDateTimeFormat());

            IEnumerable<User> users = null;
            var usersearch = await Client.User.Where(u => u.id).In(services.Select(s => (object)s.cliente)).Execute();
            if (usersearch.HasExecute)
            {
                users = usersearch.Result;
            }

            if (users == null || users.Count() == 0)
            {
                IsBusy = false;
                return;
            }

            IEnumerable<Subcategory> subcategorias = null;
            var subcategorysearch = await Client.Subcategory.Where(s => s.id).In(services.Select(s => (object)s.idsubcategoria)).Execute();
            
            if (subcategorysearch.HasExecute)
            {
                subcategorias = subcategorysearch.Result;
            }

            if (subcategorias == null)
            {
                IsBusy = false;
                return;
            }

            var idlenguaje = await Client.GetLanguage(usuario.lenguaje);
            var categories = await Client.GetCategoriesByLanguage(idlenguaje) ?? new List<Categorylanguage>();
            var subcategories = await Client.GetSubCategoriesByLanguage(idlenguaje) ?? new List<Subcategorylanguage>();

            var pendingservices = services.Where(s => s.eliminado == 0).Select(s =>
            {
                var user = users.FirstOrDefault(u => u.id == s.cliente);
                var subcategory = subcategorias.FirstOrDefault(sub => sub.id == s.idsubcategoria);
                var servicedate = DateTimeHelper.FromMySqlDateTimeFormat(s.fechadeservicio);

                var category = categories.FirstOrDefault(c => c.idcategoria == s.idcategoria);
                var subcategorylang = subcategories.FirstOrDefault(c => c.idsubcategoria == s.idsubcategoria);

                var pendingservice = new PendingService
                {
                    Categoria = category != null ? category.traduccion : s.categoria,
                    Subcategoria = subcategorylang != null ? subcategorylang.traduccion : s.subcategoria,
                    Id = s.id,
                    Cliente = user != null ? user.nombre : "----",
                    Calificacion = user != null ? user.calificacion : 0,
                    Imagen = Client.GetPath(user?.imagen ?? ""),
                    Background = Client.GetPath(subcategory?.imagen ?? ""),
                    Distancia = "----",
                    Latitud = s.latitud,
                    Longitud = s.longitud,
                    Direccion = s.direccion,
                    ClienteId = s.cliente,
                    Fecha = servicedate,
                    DateFormat = servicedate.ToString("MM/dd/yyyy hh:mm tt"),
                    Aceptar = new Command(Aceptar_Command),
                    Cancelar = new Command(Cancelar_Command),
                    GoToInfo = new Command<PendingService>(GoToInfo_Command),
                    Descripcion = s.descripcion
                };

                Task.Run(async () =>
                {
                    var calification = await Client.GetReview(user.id);
                    pendingservice.Calificacion = calification;
                });

                Task.Run(async () =>
                {
                    var position = await Geolocation.GetLastKnownLocationAsync();
                    pendingservice.Distancia = Math.Round(position.CalculateDistance(new Location(s.latitud, s.longitud), DistanceUnits.Miles), 2).ToString() + " mi";
                });

                return pendingservice;
            });

            SetServices(pendingservices);

			IsBusy = false;
		}

        /// <summary>
        /// Asigna la lista de servicios
        /// </summary>
        /// <param name="services"></param>
        public async void SetServices(IEnumerable<PendingService> services = null)
        {
            if (services == null) 
            { 
                Services = new ObservableCollection<PendingService>(); 
            }
            else 
            {
                Services = new ObservableCollection<PendingService>(services.OrderBy(s => s.Fecha)); 
            }

            await Task.Delay(10);
			IsServicesVisible = Services.Count() > 0 && !InService;
			Debug.WriteLine($"[IsServicesVisible] {IsServicesVisible} => {Services.Count()}");
		}

        private async void Cancelar_Command(object obj)
        {
            IsBusy = true;

            if (!(obj is PendingService service))
            {
                IsBusy = false;
                return;
            }

            var me = Usuario.GetUserLogin();
            if (me == null)
            {
                IsBusy = false;
                return;
            }

            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsBusy = false;
                Toast(AppResource.SinInternet);
                return;
            }

            var response = await Client.DelegateWork(service.Id, me.id);
            if (response == service.Id)
            {
                try
                {
                    Services.Remove(service);
                }
                catch { }
                ClearMap();
            }
            else if(response == -1)
            {
                Toast(AppResource.EresElUnicoQuePuedeTomarElServicio);
            }
            else
            {
                Toast(AppResource.NoPodemosDelegarElServicio);
            }

            IsBusy = false;
        }

        private async void Aceptar_Command(object obj)
        {
            if (!(obj is PendingService service)) return;
            if (!await Confirm($"{AppResource.AceptarServicio} {service.Subcategoria}?"))
            {
                return;
            }

            IsBusy = true;

            var me = Usuario.GetUserLogin();
            if (me == null)
            {
                Toast(AppResource.ErrorServer);
                IsBusy = false;
                return;
            }

            var stripe = await App.GetStripeAsync();
            if (stripe == null)
            {
                Toast(AppResource.ErrorServer);
                IsBusy = false;
                return;
            }

            var id = service.Id;

            var lastdataservice = await Client.Requestservice.Get(id);

            if(lastdataservice == null)
            {
                Toast(AppResource.ErrorServer);
                IsBusy = false;
                return;
            }

            if (lastdataservice.trabajador != me.id)
            {
                Toast(AppResource.ErrorServer);
                await LoadPendingServices();
                IsBusy = false;
                return;
            }

            if (lastdataservice.idestadoservicio != (int)EstadoServicio.Pendiente)
            {
                if (lastdataservice.idestadoservicio == (int)EstadoServicio.Cancelado) Toast(AppResource.ServicioCancelado);
                else Toast(AppResource.ErrorServer);
                IsBusy = false;
                await LoadPendingServices();
                return;
            }

            var hito = await Client.Hito.Where(new Hito
            {
                trabajoterminado = 1,
                idsolicitudservicio = lastdataservice.id
            }) ?? new List<Hito>();

            var pendingforpay = hito.FirstOrDefault(h => h.estado == (int)HitoStatus.AuthorizedFunds);
            if(pendingforpay == null)
            {
                Toast(AppResource.EsteServicioNoCuentaConFondosParaRealizarse);
                await Client.ChangeServiceStatus(service.Id, EstadoServicio.Cancelado, service.ClienteId, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0);
                await LoadPendingServices();
                IsBusy = false;
                return;
            }

            var charge = await stripe.GetChargeAsync(pendingforpay.chargeid);
            if(charge == null)
            {
                IsBusy = false;
                await Client.ChangeServiceStatus(service.Id, EstadoServicio.Cancelado, service.ClienteId, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0);
                await LoadPendingServices();
                Toast(AppResource.EsteServicioNoCuentaConFondosParaRealizarse);
                return;
            }

            if(charge.Refunds?.Data != null)
            {
                if(charge.Refunds.Data.Count > 0)
                {
                    Toast(AppResource.EsteServicioNoCuentaConFondosParaRealizarse);
                    await Client.ChangeServiceStatus(service.Id, EstadoServicio.Cancelado, service.ClienteId, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0);
                    await LoadPendingServices();
                    IsBusy = false;
                    return;
                }
            }

            if (!await Client.ChangeServiceStatus(id, EstadoServicio.Aceptado, me.id, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0))
            {
                Toast(AppResource.ErrorServicio);
                IsBusy = false;
                return;
            }

			await SocketIO?.Send($"ConfirmService-{service.ClienteId}", new Dictionary<string, string>
            {
                { "idserviciosolicitado", service.Id.ToString() }
            });

			await SocketIO?.Send($"Activity-{service.Id}", new Dictionary<string, string>
					{
						{ "idestadoservicio", ((int)EstadoServicio.Aceptado).ToString() }
					});

			if (Services == null) return;
            var index = Services.IndexOf(service);
            if(index > -1)
            {
                try
                {
                    Services.RemoveAt(index);
                }
                catch { }
            }
            await SearchService();
            IsBusy = false;
        }

        /// <summary>
        /// Cambia el estado de un servicio a traves de una confirmacion hacia el usuario
        /// </summary>
        /// <param name="estadoservicio"></param>
        /// <param name="confirmation"></param>
        /// <returns></returns>
        public async Task<bool> ChangeStatusByConfirmation(EstadoServicio estadoservicio, string confirmation)
        {
            if (CurrentService == null) return false;
            var me = Usuario.GetUserLogin();
            if (me == null) return false;
            if (CurrentService.idestadoservicio >= (int) estadoservicio)
            {
                return true;
            }

            if (await Confirm(confirmation))
            {
                if(Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    Toast(AppResource.SinInternet);
                    return false;
                }
                var currentserivce = await Client.Requestservice.Get(CurrentService.id);
                if (currentserivce == null)
                {
                    Toast(AppResource.CantRetrieveServiceStatus);
                    return false;
                }

                var cliente = await CurrentService.GetCliente();
                if (cliente == null)
                {
                    IsBusy = false;
                    Toast(AppResource.CantRetrieveServiceStatus);
                    return false;
                }

                if (currentserivce.idestadoservicio == (int)EstadoServicio.Cancelado)
                {
                    IsBusy = false;
                    Toast(string.Format(AppResource.UserCancelService, cliente.nombre));
                    SetDetailPage(new HomePage());
                    return false;
                }

                var status = await Client.ChangeServiceStatus(CurrentService.id, estadoservicio, me.id, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0);
                if (status)
                {
                    await SocketIO?.Send(ActivityChannel, new Dictionary<string, string>
                    {
                        { "idestadoservicio", ((int)estadoservicio).ToString() }
                    });
                }
                return status;
            }
            return false;
        }

        /// <summary>
        /// Evento que se lanza cuando se cambia el estado de la lista de compras a activado
        /// </summary>
        private async void OnBuyedList()
        {
            IsBusy = true;

            if(!await VerifyToolsTicket())
            {
                IsBusy = false;
                return;
            }

            if (await ChangeStatusByConfirmation(EstadoServicio.HerramientasCompradas, AppResource.TerminarListaCompras))
            {
                var tickets = await Client.Ticket.Where(new Ticket
                {
                    idsolicitudservicio = CurrentService.id
                });
                IsTicketUploadVisible = tickets != null && tickets.Count() > 0;
            }
            else
            {
                IsToolListCompleted = false;
            }

            IsBusy = false;
        }

        /// <summary>
        /// Verifica que se haya creado un hito y un ticket para la lista de herramientas
        /// </summary>
        /// <returns></returns>
        private async Task<bool> VerifyToolsTicket()
        {
            if (CurrentService == null)
            {
                return false;
            }

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Toast(AppResource.RequeresInternet);
                return false;
            }

            var tools = await Client.Accessory.Where(new Accessory
            {
                idsolicitudservicio = CurrentService.id
            });

            if (tools != null && tools.Count(t => !t.costo.HasValue) > 0)
            {
                Toast(AppResource.HerramientasSinPrecio);
                await GoAccesoriesView();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Navega hacia la vista para subir los ticket
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void GoUploadTickets_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (CurrentService == null) return;
            await Navigation.PushAsync(new TicketListPage
            {
                BindingContext = new TicketListViewModel(CurrentService.id)
            });
        }

        /// <summary>
        /// En ruta a la direccion
        /// </summary>
        private async void OnRouteToAddress()
        {
            if (CurrentService == null) return;
            IsBusy = true;
            if (await ChangeStatusByConfirmation(EstadoServicio.EnCaminoADomicilio, AppResource.ComenzarRuta))
            {
                await StartServiceThreads();
            }
            else
            {
                InRouteToAddress = false;
            }
            IsBusy = false;
        }

        /// <summary>
        /// El tasker llego a la ubicacion
        /// </summary>
        private async void OnTaskerArrivalToAddress()
        {
            IsBusy = true;
            if (await ChangeStatusByConfirmation(EstadoServicio.LlegadaADomicilio, AppResource.LlegadaDestino))
            {
                TaskerCanConfirmatingService();
            }
            else
            {
                TaskerArrivalToAddress = false;
            }
            IsBusy = false;
        }

        /// <summary>
        /// Tasker puede confirmar el servicio
        /// </summary>
        private void TaskerCanConfirmatingService()
        {
            if (CurrentService == null) return;
            IsBusy = true;
            IsGpsTraking = false;
            CountForWaitConfirmation = new TimeSpan(0, 0, 0);
            IsCountTimeLabelVisible = false;
            ArrivalHasBeenConfirmed = true;
            StartWork = new Command(StartWork_Clicked);
            var variable = DataBase.GetVariable();
            variable.StartWaitConfirmation = null;
            DataBase.SetVariable(variable);
            IsBusy = false;
        }

        /// <summary>
        /// Realiza la accion para iniciar el servicio
        /// </summary>
        /// <param name="obj"></param>
        private async void StartWork_Clicked(object obj)
        {
            if (InternetValidator != null && !InternetValidator.Rule())
            {
                Toast(InternetValidator.ErrorMessage);
                return;
            }

            IsBusy = true;
            if (CurrentService == null) return;

            var stripe = await App.GetStripeAsync();
            if (stripe == null)
            {
                IsBusy = false;
                Toast(AppResource.ErrorServer);
                return;
            }

            var hito = await Client.Hito.Where(new Hito
            {
                trabajoterminado = 1,
                idsolicitudservicio = CurrentService.id
            }) ?? new List<Hito>();

            var pendingforpay = hito.FirstOrDefault(h => h.estado == (int)HitoStatus.AuthorizedFunds);
            if (pendingforpay == null)
            {
                await ResetView(CurrentService);
                IsBusy = false;
                return;
            }

            var charge = await stripe.GetChargeAsync(pendingforpay.chargeid);
            if (charge == null)
            {
                await ResetView(CurrentService);
                IsBusy = false;
                return;
            }

            if (charge.Refunds?.Data != null)
            {
                if (charge.Refunds.Data.Count > 0)
                {
                    await ResetView(CurrentService);
                    IsBusy = false;
                    return;
                }
            }

            if (await ChangeStatusByConfirmation(EstadoServicio.TrabajoIniciado, AppResource.DeseasIniciarServicio))
            {
                StopGpsTraking();
                StopServiceThreads();
                SetDistanceToLocationService();
                if (!await GoToStartPage(CurrentService.id))
                {
                    Toast(AppResource.SinContinuar);
                    IsBusy = false;
                    return;
                }
            }
            else
            {
                Toast(AppResource.SinContinuar);
                IsBusy = false;
                return;
            }
            IsBusy = false;
        }

        /// <summary>
        /// Asigna la distancia a la ubicación del servicio
        /// </summary>
        private async void SetDistanceToLocationService()
        {
            try
            {
                await Client.Service.Update(CurrentService.id, new Dictionary<string, string>
                {
                    { nameof(Requestservice.aceptadoaladistanciade), DistanceToLocation.ToString() }
                });
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Resetea la vista del servicio
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private async Task ResetView(Requestservice service)
        {
            if (service == null) return;
            Toast(AppResource.EsteServicioNoCuentaConFondosParaRealizarse);
            await Client.ChangeServiceStatus(service.id, EstadoServicio.Cancelado, service.cliente, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0);
            SetDetailPage(new WaitingServicePage
            {
                BindingContext = new WaitingServiceViewModel(service)
            });
        }

        #region Notified Property ClientImage
        /// <summary>
        /// ClientImage
        /// </summary>
        private string clientImage;
        public string ClientImage
        {
            get { return clientImage; }
            set { clientImage = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ClientName
        /// <summary>
        /// ClientName
        /// </summary>
        private string clientname;
        public string ClientName
        {
            get { return clientname; }
            set { clientname = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Address
        /// <summary>
        /// Address
        /// </summary>
        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged(); }
        }
        #endregion

        /// <summary>
        /// Busca el ultimo servicio aceptado que tiene el usuario actual en la app
        /// </summary>
        private async Task<bool> SearchService()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return false;

            Debug.WriteLine($"[SearchService1] {CurrentService}");

            if (CurrentService == null)
            {
                CurrentService = await Client.GetCurrentService(usuario.Perfil, usuario.id);
            }
            Debug.WriteLine($"[SearchService2] {CurrentService}");
			InService = CurrentService != null;
            if (!InService) return false;
            IsServicesVisible = false;
			CurrentClient = await CurrentService.GetCliente();
            if(CurrentClient != null)
            {
                ClientImage = Client.GetPath(CurrentClient.imagen);
                ClientName = string.Format(AppResource.ConClient, CurrentClient.nombre);
            }
            Address = CurrentService.direccion;
            GoInfo = new ExtendCommand(GoInfo_Clicked, UserValidator, InternetValidator);
            SetServices();
			IsBusy = false;
			Toast(AppResource.EncontramosServicio);
            await GetHitosMoney();
            await Prepare();
            return true;
        }

        /// <summary>
        /// Realiza la accion para ir a la informacion
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void GoInfo_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (CurrentService == null) return;
            await Navigation.PushAsync(new DetailServicePage
            {
                BindingContext = new DetailServiceViewModel(CurrentService.id)
            });
        }

        #region Init Service

        /// <summary>
        /// Inicia las tareas del servicio
        /// </summary>
        private async Task StartServiceThreads()
        {
            StartGpsTraking();
            await SetRouteToService(Latitud, Longitud, CurrentService?.latitud ?? 0, CurrentService?.longitud ?? 0);
        }

        /// <summary>
        /// Detiene las tareas del servicio
        /// </summary>
        private void StopServiceThreads()
        {
            CloseSocket();
            StopGpsTraking();
            ClearMap();
        }

        #region Notified Property ShowToolListView
        /// <summary>
        /// ShowToolListView
        /// </summary>
        private bool showtoollistview;
        public bool ShowToolListView
        {
            get { return showtoollistview; }
            set { showtoollistview = value; OnPropertyChanged(); }
        }
        #endregion

        /// <summary>
        /// Prepara la vista con base a las acciones previas
        /// </summary>
        private async Task Prepare()
        {
            if (CurrentService == null) return;
            IsBusy = true;
            if (CurrentService.idestadoservicio >= (int)EstadoServicio.TrabajoIniciado)
            {
                await GoToStartPage(CurrentService.id);
                IsBusy = false;
                return;
            }

            MoveToRegion(new Position(Latitud, Longitud));
            SetPosition(CurrentService.latitud, CurrentService.longitud, "Service");
            await SetRouteToService(Latitud, Longitud, CurrentService.latitud, CurrentService.longitud);
            Indicaciones = CurrentService.detalles;
            Descripcion = CurrentService.descripcion;

            var accesorylist = (await Client.Accessory.Query(new Accessory
            {
                idsolicitudservicio = CurrentService.id
            }));

            if (accesorylist == null)
                accesorylist = new List<Accessory>();

            ShowToolListView = accesorylist.Count() > 0;

            if (ShowToolListView)
            {
                var istoollistcompleted = CurrentService.idestadoservicio >= (int)EstadoServicio.HerramientasCompradas;
                if (istoollistcompleted)
                {
                    if (!await VerifyToolsTicket())
                    {
                        IsBusy = false;
                        return;
                    }
                }
                IsToolListCompleted = istoollistcompleted;
            }

            InRouteToAddress = CurrentService.idestadoservicio >= (int)EstadoServicio.EnCaminoADomicilio;
            CloseToTheLocation = TaskerArrivalToAddress = CurrentService.idestadoservicio >= (int)EstadoServicio.LlegadaADomicilio;
            WasStartedService = CurrentService.idestadoservicio >= (int)EstadoServicio.TrabajoIniciado;
            IsBusy = false;
        }

        /// <summary>
        /// Asigna la cantidad de dinero para el trabajo
        /// </summary>
        private async Task GetHitosMoney()
        {
            IsBusy = true;
            if (CurrentService == null) return;
            var hitos = await Client.Hito.Query(new Hito
            {
                idsolicitudservicio = CurrentService.id
            }) ?? new List<Hito>();
            var cantidad = hitos.Sum(h => h.cantidad);
            Fondos = $"Fondos ${cantidad}";
            IsBusy = false;
        }

        #region GPS

        public bool IsGpsTraking { get; set; }

        /// <summary>
        /// Inicia el thread para obtener el gps cada cierto tiempo
        /// </summary>
        /// <param name="time"></param>
        private void StartGpsTraking()
        {
            IsGpsTraking = true;

			Device.StartTimer(TimeSpan.FromSeconds(GpsTimeUpdates), () =>
            {
                Task.Run(async () =>
                {
                    var usuario = Usuario.GetUserLogin();
                    if (usuario == null) return;
                    var nombre = usuario.nombre;
                    await GetLocation(async (location) =>
                    {
                        CheckIfArrivedToLocation(location);
                        SetPosition(location.Latitude, location.Longitude, usuario.nombre);
                        MoveToRegion(new Position(location.Latitude, location.Longitude));
                        await SocketIO?.Send(GpsChannel, new Dictionary<string, string>
                        {
                            { "latitud", location.Latitude.ToString() },
                            { "longitud", location.Longitude.ToString() },
                            { "nombre", nombre }
                        });
                    });
                });
                return IsGpsTraking && !CloseToTheLocation;
            });
        }

        /// <summary>
        /// Revisa si el tasker ha llegado al lugar
        /// </summary>
        /// <param name="location"></param>
        private void CheckIfArrivedToLocation(Location location)
        {
            if (CurrentService == null) return;
            DistanceToLocation = location.CalculateDistance(CurrentService.latitud, CurrentService.longitud, DistanceUnits.Kilometers) * 1000;
            CloseToTheLocation = DistanceToLocation <= 30;
        }

        /// <summary>
        /// Termina el thread del gps
        /// </summary>
        private void StopGpsTraking()
        {
            IsGpsTraking = false;
        }
        #endregion

        #region Socket
        /// <summary>
        /// Conecta al socket
        /// </summary>
        public async void ConnectToSocket()
        {
            SocketIO = await SocketFactory.Instance.Resolve();
            SocketIO.ConnectionStatus += SocketIO_ConnectionStatus;
            SocketIO.MessageReceived += SocketIO_MessageReceived;
            await SocketIO.Subscribe(ActivityChannel, NewServiceChannel, ServiceChannel);
        }

        private void SocketIO_ConnectionStatus(object sender, bool e)
        {
            if (!e)
                Toast(AppResource.OffLine);
        }

        /// <summary>
        /// Cierra la sesion del socket
        /// </summary>
        private void CloseSocket()
        {
            System.Diagnostics.Debug.WriteLine("Close", $"Socket");
            SocketIO?.Close();
        }

        /// <summary>
        /// Message received from socket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketIO_MessageReceived(object sender, Message e)
        {
            System.Diagnostics.Debug.WriteLine(e.RawText, e.Channel);
            Dictionary<string, string> data = null;
            try
            {
                data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Text);
            }
            catch { }

            if (data == null)
            {
                System.Diagnostics.Debug.WriteLine("data is null", e.Channel);
                return;
            }

            if(e.Channel == ActivityChannel)
            {
                SetActivity(data);
            }
            else if(e.Channel == NewServiceChannel)
            {
                SetNewService(data);
            }
            else if (e.Channel == ServiceChannel)
            {
                ServiceHandler(data);
            }
        }

        private void ServiceHandler(Dictionary<string, string> data)
        {
            if (!data.ContainsKey(ServiceModel.Action)) return;
            if (!int.TryParse(data[ServiceModel.Action], out int idestadoservicio)) return;
            if (!int.TryParse(data[ServiceModel.IdKey], out int id)) return;
            var list = Services.ToList();
            var service = list.FirstOrDefault(s => s.Id == id);
            if (service == null) return;
            try
            {
                list.Remove(service);
            }
            catch { }
            Services = new ObservableCollection<PendingService>(list);
            /*
            var estadoservicio = (EstadoServicio)idestadoservicio;
            var factory = new EnumFactory<EstadoServicio, ISocketService>("EmergencyTask.ViewModel.Business.SocketServiceState", "Servicio");
            var socketaction = factory.Resolve(estadoservicio);
            if (socketaction == null) return;
            var result = socketaction.Action(id, Services);
            SetServices(result);
            */
        }

        private async void SetNewService(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("idsolicitudservicio")) return;
            await GetNotifications();
            await LoadPendingServices();
        }

        private void SetActivity(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("idestadoservicio")) return;
            int.TryParse(data["idestadoservicio"].ToString(), out int idestadoservicio);
            if(idestadoservicio == (int)EstadoServicio.TrabajoIniciado)
                WasStartedService = true;
        }
        #endregion

        #region Map
        /// <summary>
        /// Asigna una posicion GPS en el mapa
        /// </summary>
        /// <param name="latitud"></param>
        /// <param name="longitud"></param>
        private void SetPosition(double latitud, double longitud, string label)
        {
            if (Mapa == null) return;
            Device.BeginInvokeOnMainThread(() =>
            {
                var pin = new Pin
                {
                    Position = new Position(latitud, longitud),
                    Address = label ?? " ",
                    Label = label ?? " "
                };
                var inmap = Mapa.Pins.FirstOrDefault(p => p.Label == label);
                if (inmap != null)
                    Mapa.Pins.Remove(inmap);
                Mapa.Pins.Add(pin);
            });
        }

        /// <summary>
        /// Crea la ruta del punto a al punto b
        /// </summary>
        private async Task SetRouteToService(double latitud, double longitud, double tolatitud, double tolongitud)
        {
            if (string.IsNullOrEmpty(GoogleMapsKey)) return;
            var route = await Client.GoogleMapsApiRoute(GoogleMapsKey, latitud, longitud, tolatitud, tolongitud);
            if(route == null || route.Count <= 1) return;
            if (Mapa == null) return;
            if (Mapa.Polylines.Count > 0)
                Device.BeginInvokeOnMainThread(() =>
                {
                     Mapa.Polylines.Clear();
                });
            
            var accentcolor = (Color)App.Resources["Accent"];
            var polyline = new Polyline
            {
                StrokeColor = accentcolor,
                StrokeWidth = 2
            };

            foreach (var item in route)
            {
                polyline.Positions.Add(item);
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    Mapa.Polylines.Add(polyline);
                }
                catch { }
            });
        }

        /// <summary>
        /// Limpia el mapa de todos los elementos
        /// </summary>
        private void ClearMap()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Mapa == null) return;
                try
                {
                    Mapa.Polylines.Clear();
                    Mapa.Pins.Clear();
                }
                catch { }
            });
        }

        #endregion

        #endregion
    }
}