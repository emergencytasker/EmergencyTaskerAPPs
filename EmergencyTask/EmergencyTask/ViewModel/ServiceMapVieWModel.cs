using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Plugin.Net.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace EmergencyTask.ViewModel
{
    public class ServiceMapVieWModel : ViewModelBase
    {

        #region Notified Property GoDetail
        /// <summary>
        /// GoDetail
        /// </summary>
        private ExtendCommand godetail;
        public ExtendCommand GoDetail
        {
            get { return godetail; }
            set { godetail = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapChat
        /// <summary>
        /// TapChat de la propiedad bindable
        /// </summary>
        private Command tapchat;
        public Command TapChat
        {
            get { return tapchat; }
            set { tapchat = value; OnPropertyChanged(); }
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

        #region BindableProperty BtnStartService
        /// <summary>
        /// BtnIniciar de la propiedad bindable
        /// </summary>
        private ExtendCommand btnstartservice;
        public ExtendCommand BtnStartService
        {
            get { return btnstartservice; }
            set { btnstartservice = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Tiempo
        /// <summary>
        /// Tiempo de la propiedad bindable
        /// </summary>
        private string tiempo;
        public string Tiempo
        {
            get { return tiempo; }
            set { tiempo = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Distancia
        /// <summary>
        /// Distancia de la propiedad bindable
        /// </summary>
        private string distancia;
        public string Distancia
        {
            get { return distancia; }
            set { distancia = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty ProgressBar
        /// <summary>
        /// ProgressBar de la propiedad bindable
        /// </summary>
        private double progessbar;
        public double ProgressBar
        {
            get { return progessbar; }
            set { progessbar = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty CircleColor1
        /// <summary>
        /// CircleColor1 de la propiedad bindable
        /// </summary>
        private Color circlecolor1;
        public Color CircleColor1
        {
            get { return circlecolor1; }
            set { circlecolor1 = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty CircleColor2
        /// <summary>
        /// CircleColor2 de la propiedad bindable
        /// </summary>
        private Color circlecolor2;
        public Color CircleColor2
        {
            get { return circlecolor2; }
            set { circlecolor2 = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty CircleColor3
        /// <summary>
        /// CircleColor3 de la propiedad bindable
        /// </summary>
        private Color circlecolor3;
        public Color CircleColor3
        {
            get { return circlecolor3; }
            set { circlecolor3 = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty CircleColor4
        /// <summary>
        /// CircleColor4 de la propiedad bindable
        /// </summary>
        private Color circlecolor4;
        public Color CircleColor4
        {
            get { return circlecolor4; }
            set { circlecolor4 = value; OnPropertyChanged(); }
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

        #region Notified Property RefreshActions
        /// <summary>
        /// RefreshActions
        /// </summary>
        private ExtendCommand refreshactions;
        public ExtendCommand RefreshActions
        {
            get { return refreshactions; }
            set { refreshactions = value; OnPropertyChanged(); }
        }
        #endregion

        public Map Mapa { get; set; }

        public bool Activity1
        {
            set
            {
                if(value)
                {
                    CircleColor2 = (Color) App.Resources["Accent"];
                    ProgressBar = 0.34;
                }
            }
        }

        public bool Activity2
        {
            set
            {
                if (value)
                {
                    CircleColor3 = (Color)App.Resources["Accent"];
                    ProgressBar = 0.67;
                }
            }
        }

        private bool activity3;
        public bool Activity3
        {
            get { return activity3; }
            set
            {
                activity3 = value;
                if (value)
                {
                    CircleColor4 = (Color)App.Resources["Accent"];
                    ProgressBar = 1;
                    OnPropertyChanged();
                }
            }
        }

        public Requestservice CurrentService { get; set; }
        public ISocket SocketIO { get; set; }
        private string GpsChannel { get; set; }
        private string ActivityChannel { get; set; }
        public string GoogleMapsKey { get; set; }
        public string Phone { get; set; }
        public User Trabajador { get; set; }

        public ServiceMapVieWModel()
        {

        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            if (CurrentService == null) return;

            BtnStartService = new ExtendCommand(BtnStartService_Clicked, new InternetValidator());
            GoDetail = new ExtendCommand(GoDetail_Clicked, new InternetValidator());
            GoHitos = new ExtendCommand(GoHitos_Clicked, new InternetValidator(), new UserValidator());
            TapChat = new Command(TapChat_Clicked);
            BtnCall = new Command(BtnCall_clicked);
            RefreshActions = new ExtendCommand(RefreshActions_Command, new InternetValidator());

            GpsChannel = $"GPS-{CurrentService.id}";
            ActivityChannel = $"Activity-{CurrentService.id}";

            GoogleMapsKey = await GetVar<string>("googlemapskey");

            await Load();
            
            ConnectToSocket();
            await GetTaskerPosition();
        }

        /// <summary>
        /// Implamentar logica tapchat
        /// </summary>
        /// <param name="obj"></param>
        private async void TapChat_Clicked(object obj)
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            if (CurrentService == null) return;
            var idcliente = 0;
            var idtrabajador = 0;
            if (usuario.Perfil == Perfil.Client)
            {
                idcliente = usuario.id;
                idtrabajador = CurrentService.trabajador;
            }
            else
            {
                idtrabajador = usuario.id;
                idcliente = CurrentService.cliente;
            }
            await Navigation.PushAsync(new ChatPage(idcliente, idtrabajador)
            {
                Title = CurrentService.categoria + " • " + CurrentService.subcategoria
            });
        }

        /// <summary>
        /// Acción para llamada
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnCall_clicked(object obj)
        {
            IsBusy = true;

            if (Trabajador == null)
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    Toast(AppResource.SinInternet);
                    IsBusy = false;
                    return;
                }

                Trabajador = await Client.User.Get(CurrentService?.trabajador ?? 0);
                if (Trabajador == null)
                {
                    Toast(AppResource.NoPodemosContinuar);
                    IsBusy = false;
                    return;
                }

                Phone = Trabajador.telefono;
            }

            if (Trabajador.telefonoverificado == 0 || string.IsNullOrEmpty(Phone))
            {
                Toast(AppResource.TrabajadorSinTelefono);
                IsBusy = false;
                return;
            }

            try
            {
                Xamarin.Essentials.PhoneDialer.Open(Phone);
            }
            catch
            {
                Toast(AppResource.NoPodemosContinuar);
            }

            IsBusy = false;
        }

        private async Task Load()
        {
            if (CurrentService == null) return;
            ProgressBar = 0;
            CircleColor1 = (Color)App.Resources["Accent"];
            CircleColor2 = Color.LightGray;
            CircleColor3 = Color.LightGray;
            CircleColor4 = Color.LightGray;
            Activity1 = CurrentService.idestadoservicio >= (int)EstadoServicio.HerramientasCompradas;
            Activity2 = CurrentService.idestadoservicio >= (int)EstadoServicio.EnCaminoADomicilio;
            Activity3 = CurrentService.idestadoservicio >= (int)EstadoServicio.LlegadaADomicilio;
            await GetHitosMoney();
            ThreadGetLocation();
            IsBusy = false;
        }

        public bool GpsThread { get; set; } = true;
        private void ThreadGetLocation()
        {
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                SocketIO?.History(GpsChannel, 1).ContinueWith(s =>
                {
                    var list = s.Result;
                    if (list == null) list = new List<Dictionary<string, string>>();
                    var result = list.FirstOrDefault();
                    SetGps(result);
                });
                return GpsThread;
            });
        }

        public override void OnDisappearing(Page page)
        {
            base.OnDisappearing(page);
            GpsThread = false;
        }

        private async void RefreshActions_Command(object arg1, IExecuteValidator[] arg2)
        {
            if(CurrentService == null) return;
            var service = await Client.Requestservice.Get(CurrentService.id);
            if (service == null) return;
            await Load();
        }

        private async Task GetTaskerPosition()
        {
            if (CurrentService == null) return;
            var trabajador = await Client.User.Get(CurrentService.trabajador);
            if (trabajador == null) return;
            await SetRoute(CurrentService.latitud, CurrentService.longitud, trabajador.latitud, trabajador.longitud);
            await GetMatrix(CurrentService.latitud, CurrentService.longitud, trabajador.latitud, trabajador.longitud);
            SetPins(CurrentService.latitud, CurrentService.longitud, trabajador.latitud, trabajador.longitud, AppResource.Servicio, trabajador.nombre);
        }

        #region Mapa

        public void SetPins(double latitud1, double longitud1, double latitud2, double longitud2, string title1, string title2)
        {
            if (Mapa == null) return;
            var service = new Position(latitud1, longitud1);
            var tasker = new Position(latitud2, longitud2);

            Device.BeginInvokeOnMainThread(() =>
            {
                Mapa.Pins.Clear();
                Mapa.Pins.Add(new Pin
                {
                    Label = title1,
                    Position = service
                });
                Mapa.Pins.Add(new Pin
                {
                    Label = title2,
                    Position = tasker
                });
            });

            MoveToPosition(tasker);
        }

        private async Task SetRoute(double latitud1, double longitud1, double latitud2, double longitud2)
        {
            try
            {
                if (Mapa == null) return;

                if (!string.IsNullOrEmpty(GoogleMapsKey))
                {
                    var route = await Client.GoogleMapsApiRoute(GoogleMapsKey, latitud1, longitud1, latitud2, longitud2);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (Mapa.Polylines.Count > 0) Mapa.Polylines.Clear();
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
                        if (polyline.Positions.Count >= 2)
                        {
                            Mapa.Polylines.Add(polyline);
                        }
                    });
                }
            }
            catch(Exception ex) { }
        }

        public async Task GetMatrix(double latitud1, double longitud1, double latitud2, double longitud2)
        {
            if (!string.IsNullOrEmpty(GoogleMapsKey))
            {
                var element = await Client.GoogleApiMatrixRequest(GoogleMapsKey, latitud1, longitud1, latitud2, longitud2);
                if (element == null)
                {
                    Distancia = "----";
                    Tiempo = "----";
                }
                else
                {
                    Distancia = element.Distance.Text;
                    Tiempo = element.Duration.Text;
                }
            }
        }

        private void MoveToPosition(Position position)
        {
            if (Mapa == null) return;
            if (position == null) return;
            Device.BeginInvokeOnMainThread(() =>
            {
                Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(3)));
            });
        }
#endregion

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
            Fondos = $"{AppResource.Fondos} ${cantidad}";
            IsBusy = false;
        }

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

        private async void GoDetail_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (CurrentService == null) return;
            await Navigation.PushAsync(new DetailServicePage
            {
                BindingContext = new DetailServiceViewModel(CurrentService.id)
            });
        }

        #region Sockets
        private async void ConnectToSocket()
        {
            SocketIO = await SocketFactory.Instance.Resolve();
            await SocketIO.Subscribe(GpsChannel, ActivityChannel);
            SocketIO.MessageReceived += SocketIO_MessageReceived;
        }

        private void SocketIO_MessageReceived(object sender, Message e)
        {
            System.Diagnostics.Debug.WriteLine(e.RawText, e.Channel);
            if (CurrentService == null) return;
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
            if (e.Channel == GpsChannel)
            {
                SetGps(data);
            }
            else if(e.Channel == ActivityChannel)
            {
                SetActivity(data);
            }
        }

        private void SetActivity(Dictionary<string, string> data)
        {
            if (data == null) return;
            if (!data.ContainsKey("idestadoservicio")) return;
            int.TryParse(data["idestadoservicio"].ToString(), out int idestadoservicio);
            Activity1 = idestadoservicio >= (int)EstadoServicio.HerramientasCompradas;
            Activity2 = idestadoservicio >= (int)EstadoServicio.EnCaminoADomicilio;
            Activity3 = idestadoservicio >= (int)EstadoServicio.LlegadaADomicilio;
        }

        private double lastlat = 0;
        private double lastlng = 0;
        private async void SetGps(Dictionary<string, string> data)
        {
            if (CurrentService == null) return;
            if (data == null || data.Count <= 0) return;
            if (!data.ContainsKey("latitud") || !data.ContainsKey("longitud")) return;
            double.TryParse(data["latitud"], out double latitud);
            double.TryParse(data["longitud"], out double longitud);
            if(lastlat == latitud && lastlng == longitud)
            {
                return;
            }
            lastlat = latitud;
            lastlng = longitud;
            await GetMatrix(CurrentService.latitud, CurrentService.longitud, latitud, longitud);
            await SetRoute(CurrentService.latitud, CurrentService.longitud, latitud, longitud);
            SetPins(CurrentService.latitud, CurrentService.longitud, latitud, longitud, AppResource.Servicio, AppResource.Taskers);
        }
        #endregion

        private async void BtnStartService_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (CurrentService == null) return;
            IsBusy = true;
            if (await ChangeStatusByConfirmation(EstadoServicio.TrabajoIniciado, AppResource.DeseasIniciarServicio))
            {
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
            if (CurrentService.idestadoservicio >= (int)estadoservicio)
            {
                return true;
            }
            if (await Confirm(confirmation))
            {
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
        /// Asigna el mapa a la propiedad Mapa
        /// </summary>
        /// <param name="mapa"></param>
        public void SetMapa(Map mapa)
        {
            Mapa = mapa;
        }
    }
}
