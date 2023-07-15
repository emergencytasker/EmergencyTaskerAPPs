using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Business;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Extensions;
using EmergencyTask.ViewModel.Validators;
using Plugin.Net.Socket;
using Plugin.Tick;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class StartServiceViewModel : ViewModelBase
    {

        #region BindableProperty Servicio
        /// <summary>
        /// Servicio de la propiedad bindable
        /// </summary>
        private string servicio;
        public string Servicio
        {
            get { return servicio; }
            set { servicio = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Tarea
        /// <summary>
        /// Tarea de la propiedad bindable
        /// </summary>
        private string tarea;
        public string Tarea
        {
            get { return tarea; }
            set { tarea = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty FotoAsistente
        /// <summary>
        /// FotoAsistente de la propiedad bindable
        /// </summary>
        private ImageSource fotoasistente;
        public ImageSource FotoAsistente
        {
            get { return fotoasistente; }
            set { fotoasistente = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty NombreAsistente
        /// <summary>
        /// NombreAsistente de la propiedad bindable
        /// </summary>
        private string nombreasistente;
        public string NombreAsistente
        {
            get { return nombreasistente; }
            set { nombreasistente = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Cronometro
        /// <summary>
        /// Cronometro de la propiedad bindable
        /// </summary>
        private TimeSpan cronometro;
        public TimeSpan Cronometro
        {
            get { return cronometro; }
            set { cronometro = value; OnPropertyChanged(); System.Diagnostics.Debug.WriteLine(value, "Cronometro"); }
        }

        public int TiempoMinimo { get; private set; }
        #endregion

        #region BindableProperty CostoTrabajo
        /// <summary>
        /// CostoTrabajo de la propiedad bindable
        /// </summary>
        private string costotrabajo;
        public string CostoTrabajo
        {
            get { return costotrabajo; }
            set { costotrabajo = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnVerInfo
        /// <summary>
        /// BtnVerInfo de la propiedad bindable
        /// </summary>
        private Command btnverinfo;
        public Command BtnVerInfo
        {
            get { return btnverinfo; }
            set { btnverinfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnFondos
        /// <summary>
        /// Fondos de la propiedad bindable
        /// </summary>
        private Command btnfondos;
        public Command BtnFondos
        {
            get { return btnfondos; }
            set { btnfondos = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnMessenger
        /// <summary>
        /// BtnMessenger de la propiedad bindable
        /// </summary>
        private Command btnmessenger;
        public Command BtnMessenger
        {
            get { return btnmessenger; }
            set { btnmessenger = value; OnPropertyChanged(); }
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

        #region Notified Property FinishWork
        /// <summary>
        /// FinishWork
        /// </summary>
        private ExtendCommand finishwork;
        public ExtendCommand FinishWork
        {
            get { return finishwork; }
            set { finishwork = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsWorkCompleted
        /// <summary>
        /// IsWorkCompleted
        /// </summary>
        private bool isworkcompleted;
        public bool IsWorkCompleted
        {
            get { return isworkcompleted; }
            set { isworkcompleted = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ReviewService
        /// <summary>
        /// ReviewService
        /// </summary>
        private ExtendCommand reviewservice;
        public ExtendCommand ReviewService
        {
            get { return reviewservice; }
            set { reviewservice = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsClient
        /// <summary>
        /// IsClient
        /// </summary>
        private bool isclient;
        public bool IsClient
        {
            get { return isclient; }
            set { isclient = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property FechaInicio
        /// <summary>
        /// FechaInicio
        /// </summary>
        private string fechainicio;
        public string FechaInicio
        {
            get { return fechainicio; }
            set { fechainicio = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property FechaFin
        /// <summary>
        /// FechaFinal
        /// </summary>
        private string fechafin;
        public string FechaFin
        {
            get { return fechafin; }
            set { fechafin = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CanFinishWork
        /// <summary>
        /// CanFinishWork
        /// </summary>
        private bool canfinishwork;
        public bool CanFinishWork
        {
            get { return canfinishwork; }
            set { canfinishwork = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnTimeList
        /// <summary>
        /// BtnTimeList de la propiedad bindable
        /// </summary>
        private ICommand btntimelist;
        public ICommand BtnTimeList
        {
            get { return btntimelist; }
            set { btntimelist = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property FechaDeFinalizacion
        /// <summary>
        /// FechaDeFinalizacion
        /// </summary>
        private string fechadefinalizacion = "--:-- -- --/--/--";
        public string FechaDeFinalizacion
        {
            get { return fechadefinalizacion; }
            set { fechadefinalizacion = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ElapsedTime
        /// <summary>
        /// ElapsedTime
        /// </summary>
        private string elapsedtime;
        public string ElapsedTime
        {
            get { return elapsedtime; }
            set { elapsedtime = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property PlayCommand
        /// <summary>
        /// PlayCommand
        /// </summary>
        private ICommand playcommand;
        public ICommand PlayCommand
        {
            get { return playcommand; }
            set { playcommand = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property StopCommand
        /// <summary>
        /// StopCommand
        /// </summary>
        private ICommand stopcommand;
        public ICommand StopCommand
        {
            get { return stopcommand; }
            set { stopcommand = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsPlaying
        /// <summary>
        /// IsPlaying
        /// </summary>
        private bool isplaying;
        public bool IsPlaying
        {
            get { return isplaying; }
            set { isplaying = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsVisible
        /// <summary>
        /// IsVisible
        /// </summary>
        private bool isvisible;
        public bool IsVisible
        {
            get { return isvisible; }
            set { isvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Refresh
        /// <summary>
        /// Refresh
        /// </summary>
        private ICommand refresh;
        public ICommand Refresh
        {
            get { return refresh; }
            set { refresh = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property HasExtras
        /// <summary>
        /// HasExtras
        /// </summary>
        private bool hasextras;
        public bool HasExtras
        {
            get { return hasextras; }
            set { hasextras = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsWaitForExtras
        /// <summary>
        /// IsWaitForExtras
        /// </summary>
        private bool iswwaitforextras;
        public bool IsWaitForExtras
        {
            get { return iswwaitforextras; }
            set { iswwaitforextras = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property RequestHoursIsActive
        /// <summary>
        /// RequestHoursIsActive
        /// </summary>
        private bool requesthoursisactive;
        public bool RequestHoursIsActive
        {
            get { return requesthoursisactive; }
            set { requesthoursisactive = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnRequestHours
        /// <summary>
        /// BtnRequestHours
        /// </summary>
        private ICommand btnrequesthours;
        public ICommand BtnRequestHours
        {
            get { return btnrequesthours; }
            set { btnrequesthours = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property HorasExtra
        /// <summary>
        /// HorasExtra
        /// </summary>
        private string horasextra;
        public string HorasExtra
        {
            get { return horasextra; }
            set { horasextra = value; OnPropertyChanged(); }
        }
        #endregion
        public Time CurrentTime { get; set; }
        public ServiceModel Service { get; set; }
        public CandidateModel User { get; set; }
        public ISocket SocketIO { get; set; }
        public string ActivityChannel
        {
            get
            {
                if (Service == null) return "";
                else return $"Activity-{Service.Id}";
            }
        }
        public string CostChannel
        {
            get
            {
                if (Service == null) return "";
                else return $"Cost-{Service.Id}";
            }
        }
        public string TimeChannel
        {
            get
            {
                if (Service == null) return "";
                return $"Time-{Service.Id}";
            }
        }
        
        public string HitosChannel
        {
            get
            {
                if (Service == null) return "";
                return $"Hitos-{Service.Id}";
            }
        }

        public string Phone { get; set; }
        public User Trabajador { get; set; }
        public Subcategory Subcategory { get; set; }
        public Requestservice CurrentService { get; set; }

        public StartServiceViewModel(ServiceModel servicemodel, CandidateModel usermodel)
        {
            Service = servicemodel;
            User = usermodel;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            if (User == null || Service == null) return;
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            IsBusy = true;
            IsClient = usuario.Perfil == Perfil.Client;
            CanFinishWork = usuario.Perfil == Perfil.Tasker;
            FotoAsistente = User.FotoAsistente;
            NombreAsistente = User.NombreAsistente;
            Servicio = AppResource.Servicio + ": " + Service.Category;
            Tarea = AppResource.Tarea + ": " + Service.SubCategory;
            Cronometro = new TimeSpan(0, 0, 0);
            CostoTrabajo = $"$ -.--";
            var validators = new IExecuteValidator[2]
            {
                new UserValidator(), new InternetValidator()
            };
            BtnTimeList = new Command(BtnTimeList_Clicked);
            Refresh = new Command(Refreshing);
            BtnVerInfo = new Command(BtnVerInfo_Clicked);
            BtnFondos = new Command(BtnFondos_Clicked);
            BtnMessenger = new Command(BtnMessenger_Clicked);
            BtnCall = new Command(BtnCall_Clicked);

            FinishWork = new ExtendCommand(FinishWork_Clicked, validators);
            ReviewService = new ExtendCommand(ReviewService_Clicked, validators);
            PlayCommand = new ExtendCommand(PlayCommand_Clicked, validators);
            StopCommand = new ExtendCommand(StopCommand_Clicked, validators);
            BtnRequestHours = new Command(BtnRequestHours_Clicked);

            IsVisible = true;
            StartSocket();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            CrossTick.Current.Tick -= Current_Tick;
            CrossTick.Current.Tick += Current_Tick;
            await Load(true);
            await CheckElapsedTime();
            await SetElapsed();
            HorasExtra = IsClient ? AppResource.HorasExtra : AppResource.HorasExtras;
            IsBusy = false;
        }

        #region Service Commands
        /// <summary>
        /// Agregar Funcionalidad de llamada
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnCall_Clicked(object obj)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (Trabajador == null)
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    Toast(AppResource.SinInternet);
                    IsBusy = false;
                    return;
                }

                var service = await Client.Requestservice.Get(Service.Id);
                if (service == null)
                {
                    Toast(AppResource.NoPodemosContinuar);
                    IsBusy = false;
                    return;
                }

                Trabajador = await Client.User.Get(service.trabajador);
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
                PhoneDialer.Open(Phone);
            }
            catch
            {
                Toast(AppResource.NoPodemosContinuar);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Boton de mensajes
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnMessenger_Clicked(object obj)
        {
            if (Service == null || User == null) return;
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            var idcliente = 0;
            var idtrabajador = 0;
            if (usuario.Perfil == Perfil.Client)
            {
                idcliente = usuario.id;
                idtrabajador = User.Id;
            }
            else
            {
                idtrabajador = usuario.id;
                idcliente = User.Id;
            }
            await Navigation.PushAsync(new ChatPage(idcliente, idtrabajador)
            {
                Title = Service.Category + " • " + Service.SubCategory
            });
        }

        /// <summary>
        /// Muestra la lista de fondos disponibles o en curso del servicio
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnFondos_Clicked(object obj)
        {
            if (Service == null) return;
            await Navigation.PushAsync(new HitoListPage
            {
                BindingContext = new HitoListViewModel(Service.Id)
            });
        }

        /// <summary>
        /// Muestra la informacion del servicio
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnVerInfo_Clicked(object obj)
        {
            if (Service == null || User == null) return;
            await GoToServiceInfoPage(Service.Id);
        }
        #endregion

        public async Task SetElapsed()
        {
            if (IsPlaying) return;
            var playdate = await SecureStorage.GetAsync("PlayDate");
            if (string.IsNullOrEmpty(playdate)) return;
            DateTime.TryParse(playdate, out DateTime date);
            var ticks = DateTime.Now.Ticks - date.Ticks;
            var seconds = (int)TimeSpan.FromTicks(ticks).TotalSeconds;
            Cronometro = TimeSpan.FromSeconds(seconds);
        }

        private async void BtnRequestHours_Clicked(object obj)
        {
            await RequestMoreHours();
        }

        private void Current_Tick(object sender, EventArgs e)
        {
            if (((int)Cronometro.TotalHours) >= Service.Time)
            {
                CrossTick.Current.Stop();
                RequestHoursIsActive = true;
                StopCommand_Clicked(null, null);
                return;
            }
            Cronometro = Cronometro.Add(TimeSpan.FromSeconds(1));
            if(Cronometro.Seconds == 0)
                NotifyChronometer();
        }


        #region Notified Property Id
        /// <summary>
        /// Id
        /// </summary>
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        #endregion

        private async Task Load(bool stop)
        {
            if (Service == null) return;
            CurrentService = (await Client.Requestservice.Where(new Requestservice
            {
                id = Service.Id
            })).LastOrDefault();
            if (CurrentService == null)
            {
                Toast(AppResource.NoPodemosContinuar);
                IsBusy = false;
                return;
            }
            Id = CurrentService.idservicio;
            FechaInicio = CurrentService.fechainicio.FromMySqlDateTimeFormat().ToString("hh:mm tt MM/dd/yy");
            FechaFin = CurrentService.fechafin.FromMySqlDateTimeFormat().ToString("hh:mm tt MM/dd/yy");
            CurrentTime = await Client.GetLastTime(Service.Id);
            await SetTime(CurrentTime, stop);
            var hito = await Client.GetCurrentHito(Service.Id);
            SetExtrasStatus(hito);
        }

        private void SetExtrasStatus(Hito hito)
        {
            if (hito == null) return;
            var status = (HitoStatus) hito.estado;
            if (hito.extras == 0) return;
            switch (status)
            {
                case HitoStatus.Created:
                    IsWaitForExtras = true;
                    break;
                case HitoStatus.AuthorizedFunds:
                    HasExtras = true;
                    break;
            }
        }

        /// <summary>
        /// Permite refrescar la vista para obtener los datos del tiempo y del costo
        /// </summary>
        /// <param name="obj"></param>
        private async void Refreshing(object obj)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsBusy = false;
                return;
            }
            await Load(false);
            await SetElapsed();
            IsBusy = false;
        }

        public override void OnDisappearing(Page page)
        {
            base.OnDisappearing(page);
            CrossTick.Current.Stop();
            IsVisible = false;
        }

        /// <summary>
        /// Detiene el tiempo cuando se hace click en stop
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void StopCommand_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if(CurrentTime == null) return;
            if (IsBusy) return;
            IsBusy = true;
            var curdate = DateTime.Now;
            var enddate = curdate.ToMySqlDateTimeFormat();
            DateTime.TryParse(await SecureStorage.GetAsync("RealPlayDate"), out DateTime realplaydate);
            var reports = new CostCalculator().Calculate(realplaydate, curdate, 100, 200);
            var tarifa = reports.Max(s => s.Tarifa);
            var elapsed = TimeSpan.FromSeconds(reports.Sum(s => s.Seconds));
            var tiempo = elapsed.ToString();
            var updated = await Client.Time.Update(CurrentTime.id, new Dictionary<string, string>
            {
                { nameof(Time.fechafin), enddate  },
                { nameof(Time.finalizado), "1" },
                { nameof(Time.costo), tarifa.ToString() },
                { nameof(Time.tiempo), tiempo }
            });
            if(updated == null || updated.finalizado != 1)
            {
                IsBusy = false;
                Toast(AppResource.ErrorServer);
                return;
            }
            SecureStorage.Remove("PlayDate");
            SecureStorage.Remove("RealPlayDate");
            
            CrossTick.Current.Stop();
            await Task.Delay(TimeSpan.FromSeconds(5));
            var last = await CheckElapsedTime();
            if (last != null) updated = last;
            await SetTime(updated, false);
            IsPlaying = false;
            IsBusy = false;
        }

        private async Task<Time> CheckElapsedTime()
        {
            if (Service == null) return null;
            var lasttime = await Client.GetLastTime(Service.Id);
            TimeSpan elapsed = TimeSpan.FromSeconds(0);
            if (lasttime != null)
                TimeSpan.TryParse(lasttime.tiempo, out elapsed);
            RequestHoursIsActive = ((int)elapsed.TotalHours) >= Service.Time;
            if (!RequestHoursIsActive)
            {
                var haspendingpayment = await HasPendingPayment();
                if (!haspendingpayment) RequestHoursIsActive = true;
            }
            return lasttime;
        }

        /// <summary>
        /// Permite iniciar el tiempo cuando se hace click en play
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void PlayCommand_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (Service == null || CurrentService == null) return;
            if (IsBusy) return;
            IsBusy = true;

            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Toast(AppResource.RequeresInternet);
                IsBusy = false;
                return;
            }

            var chargeid = await Client.GetCurrentChargeId(Service.Id);
            if (string.IsNullOrEmpty(chargeid))
            {
                IsBusy = false;
                Toast(AppResource.ErrorServer);
                return;
            }

            var date = await Client.GetDate();
            if (!date.HasValue || Service == null)
            {
                Toast(AppResource.NoContoTiempo);
                IsBusy = false;
                return;
            }

            var time = await CheckElapsedTime();
            if (time != null)
            {
                TimeSpan elapsed = TimeSpan.FromSeconds(0);
                TimeSpan.TryParse(time.tiempo, out elapsed);
                if (RequestHoursIsActive = ((int)elapsed.TotalHours) >= Service.Time)
                {
                    await RequestMoreHours();
                    IsBusy = false;
                    return;
                }
            }

            if (CurrentTime == null || CurrentTime.finalizado == 1)
            {
                CurrentTime = await Client.Time.Add(new Time
                {
                    fechainicio = date.Value.ToMySqlDateTimeFormat(),
                    trabajador = CurrentService.trabajador,
                    idsolicitudservicio = CurrentService.id,
                    chargeid = chargeid
                });
            }
            
            if(CurrentTime == null)
            {
                Toast(AppResource.NoPodemosContinuar);
                IsBusy = false;
                IsPlaying = false;
                return;
            }
             
            CurrentTime.fechainicio = date.Value.ToMySqlDateTimeFormat();
            var realdate = date.Value.Subtract(Cronometro);

            SecureStorage.Remove("PlayDate");
            SecureStorage.Remove("RealPlayDate");
            await SecureStorage.SetAsync("RealPlayDate", date.Value.ToString());
            await SecureStorage.SetAsync("PlayDate", realdate.ToString());

            Tick();

            IsBusy = false;
            IsPlaying = true;
        }

        private async void NotifyChronometer()
        {
            await SocketIO?.Send(TimeChannel, new Dictionary<string, string>
            {
                { "tiempo", Cronometro.ToString() }
            });
        }

        private async Task RequestMoreHours()
        {
            if (Service == null) return;

            var result = await Confirm($"{AppResource.TeAcabasteElTiempoDeTrabajo}, {AppResource.QuieresSolicitarMasTiempo}");
            if (!result) return;

            if (IsBusy) return;
            IsBusy = true;

            var pendingpayment = await HasPendingPayment();
            if (pendingpayment)
            {
                if (!await ReleaseCurrentHito())
                {
                    Toast(AppResource.ErrorServer);
                    return;
                }
            }

            await PopupNavigation.Instance.PushAsync(new RequestHoursPopupPage
            {
                BindingContext = new RequestHoursViewModel(CurrentService.id, CurrentService.trabajador, CurrentService.cliente)
                {
                    Success = () =>
                    {
                        Refreshing(null);
                        IsWaitForExtras = true;
                    }
                }
            });

            IsBusy = false;
        }

        private async Task<bool> HasPendingPayment()
        {
            if (Service == null) return false;
            var hitos = await Client.Hito.Where(new Hito
            {
                idsolicitudservicio = Service.Id,
                trabajoterminado = 1
            }) ?? new List<Hito>();
            IsWaitForExtras = hitos.Any(h => h.eliminado == 0 && string.IsNullOrEmpty(h.chargeid));
            return hitos.Where(h => h.eliminado == 0).Any(h => h.estado == (int)HitoStatus.AuthorizedFunds);
        }

        private async Task SetTime(Time currentTime, bool stop)
        {
            try
            {
                if (Service == null) return;
                var tiempo = await Client.GetLastTime(Service.Id);
                if (tiempo != null)
                {
                    CurrentTime = tiempo;
                    TimeSpan.TryParse(tiempo.tiempo, out TimeSpan time);
                    Cronometro = time;
                }
                else if (currentTime != null)
                {
                    CurrentTime = currentTime;
                    TimeSpan.TryParse(currentTime.tiempo, out TimeSpan time);
                    if (Cronometro == null) Cronometro = new TimeSpan(0);
                    Cronometro.Add(time);
                }
                if (Cronometro.TotalMinutes > Service.Time * 60) Cronometro = TimeSpan.FromHours(Service.Time);
                if (CurrentTime == null) return;
                IsPlaying = CurrentTime.finalizado == 0;
                if (IsPlaying)
                {
                    Tick();
                    if (stop) StopCommand_Clicked(null, null);
                }
                NotifyChronometer();
            }
            catch(Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
            }
        }

        private void Tick()
        {
            CrossTick.Current.Stop();
            CrossTick.Current.Start();
        }

        /// <summary>
        /// Dirige hacia la vista del tiempo realizado por el trabajo
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnTimeList_Clicked(object obj)
        {
            if (Service == null) return;
            await Navigation.PushAsync(new TimeWorkListPage
            {
                BindingContext = new TimeWorkListViewModel(Service.Id)
            });
        }

        /// <summary>
        /// Permite detectar un cambio en la conectividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                StopSocket();
                return;
            }
            StartSocket();
        }

        /// <summary>
        /// Review service
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void ReviewService_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            await GoToReview(Service?.Id ?? 0);
        }

        #region Sockets
        private async void StartSocket()
        {
            SocketIO = await SocketFactory.Instance.Resolve();
            SocketIO.MessageReceived += SocketIO_MessageReceived;
            var channels = new List<string> { ActivityChannel, HitosChannel };
            if (IsClient)
            {
                channels.Add(CostChannel);
                channels.Add(TimeChannel);
            }
            await SocketIO.Subscribe(channels.Where(c => !string.IsNullOrEmpty(c)).ToArray());

            foreach(string str in channels.Where(c => !string.IsNullOrEmpty(c)).ToArray())
            {
                Debug.WriteLine($"[CHANNEL] {str}");
            }
        }

        private void StopSocket()
        {
            SocketIO?.Close();
        }

        private void SocketIO_MessageReceived(object sender, Message e)
        {
            System.Diagnostics.Debug.WriteLine(e.RawText, e.Channel);
            if (Service == null) return;
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

            if (e.Channel == ActivityChannel)
            {
                SetActivity(data);
            }
            else if(e.Channel == CostChannel)
            {
                SetCost(data);
            }
            else if(e.Channel == TimeChannel)
            {
                SetTime(data);
            }
            else if (e.Channel == HitosChannel)
            {
                Refreshing(null);
            }
        }

        /// <summary>
        /// Asigna el cronometro
        /// </summary>
        /// <param name="data"></param>
        private void SetTime(Dictionary<string, string> data)
        {
            if (App.Perfil == Perfil.Tasker) return;
            if (!data.ContainsKey("tiempo")) return;
            var tiempo = data["tiempo"];
            Cronometro = TimeSpan.Parse(tiempo);
        }

        /// <summary>
        /// Asigna el costo final del tiempo
        /// </summary>
        /// <param name="data"></param>
        private void SetCost(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("costo")) return;
            if (App.Perfil == Perfil.Tasker) return;
            CostoTrabajo = data["costo"];
        }

        /// <summary>
        /// Asigna los datos de la actividad [estado de servicio]
        /// </summary>
        /// <param name="data"></param>
        private async void SetActivity(Dictionary<string, string> data)
        {
            if (Service == null) return;
            if (!data.ContainsKey("idestadoservicio")) return;
            await GetNotifications();
            var time = await Client.GetLastTime(Service.Id);
            if(time != null)
            {
                var fechafin = time.fechafin.FromMySqlDateTimeFormat();
                FechaDeFinalizacion = fechafin.ToString("hh:mm tt MM/dd/yy");
                TimeSpan.TryParse(time.tiempo, out TimeSpan timespan);
                Cronometro = timespan;
            }

            var idestadoservicio = int.Parse(data["idestadoservicio"]);
            IsWorkCompleted = idestadoservicio == (int)EstadoServicio.TrabajoTerminado;
		}
        #endregion

        /// <summary>
        /// Proceso
        /// • Se asigna la cantidad de horas laboradas al trabajo
        /// - Se debe calcular el costo final del trabajo, si es mayor al costo del trabajo, cobramos el 100%
        /// • Se cobra el trabajo
        /// • Se actualiza el estado del trabajo
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void FinishWork_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (!await ReleaseCurrentHito())
            {
                IsBusy = false;
                Toast(AppResource.SinConcluirTrabajo);
                return;
            }
            if(!await FinishService())
            {
                Toast(AppResource.SinConcluirTrabajo);
                IsBusy = false;
                return;
            }
            await SetResult();
            IsWorkCompleted = true;
            CanFinishWork = false;
            IsBusy = false;
        }

        /// <summary>
        /// Obtiene y muestra el resultado del tiempo y dinero gastado o ganado
        /// </summary>
        /// <returns></returns>
        private async Task SetResult()
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;

            var times = (await Client.Time.Where(new Time
            {
                idsolicitudservicio = Service.Id,
                finalizado = 1,
                trabajador = me.id
            })) ?? new List<Time>();

            var hitos = await Client.Hito.Where(new Hito
            {
                idsolicitudservicio = Service.Id,
                trabajoterminado = 1
            }) ?? new List<Hito>();

            if (times.Count() <= 0 || hitos.Count() <= 0) return;

            var groupbytarifa = times.GroupBy(t => t.costo);
            var elapsed = new TimeSpan(0);
            foreach (var timebytarifa in groupbytarifa)
            {
                try
                {
                    var ticks = timebytarifa.ToList().Select(s => TimeSpan.Parse(s.tiempo)).Sum(s => s.Ticks);
                    var time = TimeSpan.FromTicks(ticks);
                    elapsed = elapsed.Add(time);
                }
                catch (Exception ex)
                {
                    elapsed = new TimeSpan(0);
                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                }
            }

            ElapsedTime = elapsed.ToString();
            try
            {
                FechaDeFinalizacion = times.OrderByDescending(t => t.id).FirstOrDefault().fechafin.FromMySqlDateTimeFormat().ToString("hh:mm tt MM/dd/yy");
            }
            catch { }

            var cost = 0D;
            try
            {
                cost = hitos.Sum(h => h.costofinal);
            }
            catch { }

            if (App.Perfil == Perfil.Client)
                CostoTrabajo = "$ " + cost.ToString();
            else
                CostoTrabajo = "$ " + Math.Round(cost / 2, 2).ToString();

            await SocketIO?.Send(CostChannel, new Dictionary<string, string>
            {
                { "costo", $"$ {Math.Round(cost, 2)}" }
            });
        }

        /// <summary>
        /// Termina el servicio
        /// </summary>
        /// <returns></returns>
        private async Task<bool> FinishService()
        {
            if (Service == null) return false;
            var me = Usuario.GetUserLogin();
            if (me == null) return false;
            if (!await Client.ChangeServiceStatus(Service.Id, EstadoServicio.TrabajoTerminado, me.id, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0))
                return false;

            Debug.WriteLine($"[SocketIO] {ActivityChannel}");
            await SocketIO?.Send(ActivityChannel, new Dictionary<string, string>
            {
                { "idestadoservicio", ((int) EstadoServicio.TrabajoTerminado).ToString() }
            });

       
            return true;
        }

        /// <summary>
        /// Libera el hito actual
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ReleaseCurrentHito()
        {
            if (Service == null) return false;

            var me = Usuario.GetUserLogin();
            if (me == null)
            {
                IsBusy = false;
                return false;
            }

            if (!await CanStopTime())
            {
                IsBusy = false;
                return false;
            }

            var stripe = await App.GetStripeAsync();
            if (stripe == null)
            {
                IsBusy = false;
                Toast(AppResource.ErrorServer);
                return false;
            }

            var chargeid = await Client.GetCurrentChargeId(Service.Id);
            if (string.IsNullOrEmpty(chargeid))
            {
                IsBusy = false;
                Toast(AppResource.ErrorServer);
                return false;
            }

            var charge = await stripe.GetChargeAsync(chargeid);
            if (charge == null)
            {
                IsBusy = false;
                Toast(AppResource.ErrorServer);
                return false;
            }

            CurrentTime = await Client.GetLastTime(Service.Id);
            if (CurrentTime == null)
            {
                IsBusy = false;
                Toast(AppResource.ErrorServer);
                return false;
            }

            var times = (await Client.Time.Where(new Time
            {
                idsolicitudservicio = Service.Id,
                finalizado = 1,
                trabajador = me.id,
                chargeid = chargeid
            })) ?? new List<Time>();

            if (times.Count() == 0)
            {
                IsBusy = false;
                Toast(AppResource.ErrorServer);
                return false;
            }

            var groupbytarifa = times.GroupBy(t => t.costo);

            var cost = 0D;
            var elapsed = new TimeSpan(0);
            bool tarifabase = false;
            foreach (var timebytarifa in groupbytarifa)
            {
                try
                {
                    var ticks = timebytarifa.ToList().Select(s => TimeSpan.Parse(s.tiempo)).Sum(s => s.Ticks);
                    var time = TimeSpan.FromTicks(ticks);
                    elapsed = elapsed.Add(time);
                    if (!tarifabase)
                        if (elapsed.TotalMinutes <= 60)
                        {
                            tarifabase = true;
                            cost += timebytarifa.Key;
                        }
                        else
                            cost += new CostCalculator().Calculate(time.TotalMinutes, timebytarifa.Key);
                    else
                        cost += new CostCalculator().Calculate(time.TotalMinutes, timebytarifa.Key);
                }
                catch (Exception ex)
                {
                    cost = 0D;
                    elapsed = new TimeSpan(0);
                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                }
            }

            if (cost == 0)
            {
                Toast(AppResource.ErrorServer);
                IsBusy = false;
                return false;
            }

            var stripecost = charge.Amount / 100D;

            if (elapsed.TotalHours > Service.Time || cost > stripecost)
            {
                elapsed = new TimeSpan((int)Service.Time, 0, 0);
                cost = stripecost;
            }

            if (cost <= 0)
            {
                Toast(AppResource.NoDetuvoTiempo);
                IsBusy = false;
                return false;
            }

            if (!await ReleaseHito(cost))
            {
                Toast(AppResource.NoFinalizoTrabajo);
                IsBusy = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Permite saber si es posible detener el tiempo y terminar el servicio
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CanStopTime()
        {
            if (Service == null)
            {
                Toast(AppResource.NoDetuvoTiempo);
                return false;
            }

            if (CurrentTime != null && CurrentTime.finalizado == 0)
            {
                Toast("First stop time");
                return false;
            }

            var date = await Client.GetDate();
            if (date == null)
            {
                Toast(AppResource.NoDetuvoTiempo);
                return false;
            }

            Subcategory = await Client.Subcategory.Get(Service.IdSubcategory);
            if (Subcategory == null)
            {
                Toast(AppResource.NoDetuvoTiempo);
                return false;
            }

            var tiempo = await Client.GetLastTime(Service.Id);
            if (tiempo == null)
            {
                Toast(AppResource.NoDetuvoTiempo);
                return false;
            }

            if (!TimeSpan.TryParse(tiempo.tiempo, out TimeSpan time))
            {
                Toast(AppResource.NoDetuvoTiempo);
                return false;
            }

            Cronometro = time;

            TiempoMinimo = await GetVar<int>("tiempominimo");
            if (TiempoMinimo <= 0)
            {
                Toast(AppResource.NoFinalizoTrabajo);
                return false;
            }

            if (Cronometro.TotalMinutes < TiempoMinimo)
            {
                Toast(string.Format(AppResource.NoHazAlcanzadoElTiempoMinimoDeServicio, TiempoMinimo));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Libera el hito por el trabajo
        /// </summary>
        /// <param name="amount">cantidad final</param>
        /// <returns></returns>
        private async Task<bool> ReleaseHito(double amount)
        {
            var hito = (await Client.Hito.Where(new Hito
            {
                idsolicitudservicio = Service.Id,
                estado = (int)HitoStatus.AuthorizedFunds,
                trabajoterminado = 1
            })).FirstOrDefault();
            if (hito == null) return true;
            var idcliente = hito.cliente;
            if (idcliente <= 0) return false;
            var cliente = await Client.User.Get(idcliente);
            if (cliente == null) return false;
            Usuario client;
            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(cliente);
                client = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(json);
            }
            catch
            {
                return false;
            }

            if (client == null) return false;
            return await client.ReleaseHito(hito.chargeid, amount, hito.id, true);
        }
    }
}