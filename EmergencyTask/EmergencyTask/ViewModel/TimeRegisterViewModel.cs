using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Business;
using EmergencyTask.ViewModel.Extensions;
using Plugin.Net.Socket;
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
    public class TimeRegisterViewModel : ViewModelBase
    {

        #region Notified Property Cronometro
        /// <summary>
        /// Cronometro
        /// </summary>
        private TimeSpan cronometro;
        public TimeSpan Cronometro
        {
            get { return cronometro; }
            set { cronometro = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CostoTrabajo
        /// <summary>
        /// CostoTrabajo
        /// </summary>
        private string costotrabajo;
        public string CostoTrabajo
        {
            get { return costotrabajo; }
            set { costotrabajo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Refresh
        /// <summary>
        /// Refresh
        /// </summary>
        private ICommand refreshcommand;

        public ICommand RefreshCommand
        {
            get { return refreshcommand; }
            set { refreshcommand = value; OnPropertyChanged(); }
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

        #region Notified Property Servicio
        /// <summary>
        /// Servicio
        /// </summary>
        private string servicio;
        public string Servicio
        {
            get { return servicio; }
            set { servicio = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Tarea
        /// <summary>
        /// Tarea
        /// </summary>
        private string tarea;
        public string Tarea
        {
            get { return tarea; }
            set { tarea = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property FinishWork
        /// <summary>
        /// FinishWork
        /// </summary>
        private Command finishwork;
        public Command FinishWork
        {
            get { return finishwork; }
            set { finishwork = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ReviewService
        /// <summary>
        /// ReviewService
        /// </summary>
        private ICommand reviewservice;
        public ICommand ReviewService
        {
            get { return reviewservice; }
            set { reviewservice = value; OnPropertyChanged(); }
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

        #region Notified Property EndDateChanged
        /// <summary>
        /// EndDateChanged
        /// </summary>
        private ICommand enddatechanged;
        public ICommand EndDateChanged
        {
            get { return enddatechanged; }
            set { enddatechanged = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property StartDate
        /// <summary>
        /// StartDate
        /// </summary>
        private DateTime startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public DateTime StartDate
        {
            get { return startdate; }
            set { startdate = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property StartTime
        /// <summary>
        /// StartTime
        /// </summary>
        private TimeSpan starttime;
        public TimeSpan StartTime
        {
            get { return starttime; }
            set { starttime = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property EndDate
        /// <summary>
        /// EndDate
        /// </summary>
        private DateTime enddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public DateTime EndDate
        {
            get { return enddate; }
            set { enddate = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property EndTime
        /// <summary>
        /// EndTime
        /// </summary>
        private TimeSpan enddatime;
        public TimeSpan EndTime
        {
            get { return enddatime; }
            set { enddatime = value; OnPropertyChanged(); EndDateChanged_Command(null); }
        }
        #endregion

        #region Notified Property IsTotalValid
        /// <summary>
        /// IsTotalValid
        /// </summary>
        private bool istotalvalid;
        public bool IsTotalValid
        {
            get { return istotalvalid; }
            set { istotalvalid = value; OnPropertyChanged(); }
        }

        #endregion

        public ISocket SocketIO { get; set; }
        public ServiceModel Service { get; set; }
        public CandidateModel User { get; set; }
        public User Trabajador { get; set; }
        public string Phone { get; set; }
        public List<Report> Times { get; set; }
        private DateTime Start { get; set; }
        private DateTime End { get; set; }
        public double Total { get; set; }

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

        public TimeRegisterViewModel(ServiceModel servicemodel, CandidateModel usermodel)
        {
            Service = servicemodel;
            User = usermodel;
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            if (User == null || Service == null) return;
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            IsBusy = true;
            IsClient = usuario.Perfil == Perfil.Client;
            CanFinishWork = usuario.Perfil == Perfil.Tasker;
            Servicio = AppResource.Servicio + ": " + Service.Category;
            Tarea = AppResource.Tarea + ": " + Service.SubCategory;
            Cronometro = new TimeSpan(0, 0, 0);
            CostoTrabajo = $"$ -.--";
            BtnVerInfo = new Command(BtnVerInfo_Clicked);
            BtnFondos = new Command(BtnFondos_Clicked);
            BtnMessenger = new Command(BtnMessenger_Clicked);
            BtnCall = new Command(BtnCall_Clicked);
            FinishWork = new Command(FinishWork_Clicked, (args) =>
            {
                if (args == null) return false;
                bool.TryParse(args.ToString(), out bool status);
                return status;
            });

            FinishWork.CanExecute(false);
            FinishWork.ChangeCanExecute();

            ReviewService = new Command(ReviewService_Clicked);
            EndDateChanged = new Command(EndDateChanged_Command);
            StartSocket();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsBusy = false;
        }

        /// <summary>
        /// Review service
        /// </summary>
        /// <param name="arg1"></param>
        private async void ReviewService_Clicked(object arg1)
        {
            IsBusy = true;
            await GoToReview(Service?.Id ?? 0);
            IsBusy = false;
        }

        /// <summary>
        /// Accion para cuando la fecha de fin cambia o es elegida
        /// </summary>
        /// <param name="obj"></param>
        private async void EndDateChanged_Command(object obj)
        {
            Start = StartDate.Add(StartTime);
            End = EndDate.Add(EndTime);

            if (End < Start)
            {
                Toast(AppResource.InvalidFinishDate);
                IsBusy = false;
                FinishWork.CanExecute(false);
                return;
            }

            var time = End.Subtract(Start);
            if (time.TotalSeconds < 0)
            {
                Toast(AppResource.InvalidFinishDate);
                IsBusy = false;
                FinishWork.CanExecute(false);
                return;
            }

            var minutos = time.TotalMinutes;
            var minimo = await GetVar<int>("tiempominimo");
            if (minutos <= minimo)
            {
                Toast(string.Format(AppResource.NoHazAlcanzadoElTiempoMinimoDeServicio, minimo));
                IsBusy = false;
                FinishWork.CanExecute(false);
                return;
            }

            // var maximo = await GetVar<int>("tiempomaximo");
            var maximo = 8D;
            if (time.TotalHours > maximo)
            {
                Toast(AppResource.MensajeTiempo);
                IsBusy = false;
                FinishWork.CanExecute(false);
                return;
            }

            Times = new CostCalculator().Calculate(Start, End, 100, 200);

            var total = Times.Sum(s => s.Cost);

            if (total == 0)
            {
                Toast(AppResource.ErrorServer);
                IsBusy = false;
                FinishWork.CanExecute(false);
                return;
            }

            IsTotalValid = total > 0;

            Total = total;
            CostoTrabajo = $"${Math.Round(Total / 2, 2)}";
            Cronometro = time;

            FinishWork.CanExecute(true);
            IsBusy = false;
        }

        /// <summary>
        /// Proceso
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void FinishWork_Clicked(object arg1)
        {
            if (Service == null) return;
            if (Times == null || Times.Count == 0) return;
            if (Total <= 0) return;

            if (IsBusy) return;
            IsBusy = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Toast(AppResource.SinInternet);
                IsBusy = false;
                return;
            }

            var client = await Client.User.Get(User.Id);
            if (client == null)
            {
                IsBusy = false;
                Toast("Client Data Error");
                return;
            }

            var modo = await GetVar<string>("modo");

            if (string.IsNullOrEmpty(modo))
            {
                Toast("Config Stripe Error");
                IsBusy = false;
                return;
            }

            var stripeuserindb = (await Client.Stripeuser.Query(new Stripeuser
            {
                idusuario = client.id,
                modo = modo
            }) ?? new List<Stripeuser>()).FirstOrDefault();

            if (stripeuserindb == null || stripeuserindb.id < 0)
            {
                Toast("Stripe Id Client Error");
                IsBusy = false;
                return;
            }

            var customerid = stripeuserindb.customer;

            var stripe = await App.GetStripeAsync();
            if (stripe == null)
            {
                IsBusy = false;
                Toast("Stripe Config Error");
                return;
            }

            var chargeid = await Client.GetCurrentChargeId(Service.Id);
            if (string.IsNullOrEmpty(chargeid))
            {
                IsBusy = false;
                Toast("Stripe Charge Id Error");
                return;
            }

            var charge = await stripe.GetChargeAsync(chargeid);
            if (charge == null)
            {
                IsBusy = false;
                Toast("Stripe Charge Not Found Error");
                return;
            }

            var stripecost = charge.Amount / 100D;
            var faltante = 0D;
            if (Total > stripecost) faltante = Total - stripecost;
            else stripecost = Total;

            if (stripecost <= 0)
            {
                Toast("Stripe Charge $0 Error");
                IsBusy = false;
                return;
            }

            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Toast(AppResource.SinInternet);
                IsBusy = false;
                return;
            }

            var time = Client.Time.Add(new Time
            {
                chargeid = chargeid,
                costo = Total,
                fechafin = End.ToMySqlDateTimeFormat(),
                fechainicio = Start.ToMySqlDateTimeFormat(),
                finalizado = 1,
                idsolicitudservicio = Service.Id,
                tiempo = Cronometro.ToString(),
                eliminado = 0,
                trabajador = Service.Tasker
            });

            if(time == null || time.Id <= 0)
            {
                Toast("Time Data Base Error");
                IsBusy = false;
                return;
            }

            // liberamos el hito actual
            if (!await ReleaseHito(stripecost))
            {
                Toast("Stripe Release Charge Error");
                IsBusy = false;
                return;
            }

            await FinishService();

            if (faltante <= 0D) 
            {
                IsWorkCompleted = true;
                CanFinishWork = false;
                IsBusy = false;
                return;
            }

            // tenemos que crear el hito por el monto faltante
            var description = Service.Category + " • " + Service.SubCategory + "#" + Service.Id;
            if (!await AddHito(stripe, faltante, customerid, description, client.email))
                Toast(string.Format(AppResource.NewHitoMessage, faltante));

            try
            {
                SocketIO?.Send(CostChannel, new Dictionary<string, string>
                {
                    { "costo", $"$ {Math.Round(Total, 2)}" }
                });

                await SocketIO?.Send(TimeChannel, new Dictionary<string, string>
                {
                    { "tiempo", Cronometro.ToString() }
                });
            }
            catch { }

           _reddems:

            Debug.WriteLine($"[Tasker] {Service.Tasker}");
            var redeems = await EmergencyTask.API.Client.Redeem.Where(new EmergencyTask.API.ER.Redeem()
            {
                idrecompensas = 2,
                idusuario = Service.Tasker
            });

            if(redeems == null || redeems.Count() == 0)
            {
                var redeem = await EmergencyTask.API.Client.Redeem.Add(new EmergencyTask.API.ER.Redeem()
                {
                    idrecompensas = 2,
                    idusuario = Service.Tasker
                });

                goto _reddems;
            }

            foreach (var redeem in redeems)
            {
                if (redeem.reclamada == 0 && redeem.realizado <= redeem.variable)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    dic.Add("realizado", (redeem.realizado + 1).ToString());
                    await EmergencyTask.API.Client.Redeem.Update(redeem.id, dic);
                    Debug.WriteLine($"[redeem] {redeem.idusuario} | {redeem.realizado} | {redeem.referencia}");
                }
            }

        _reddems2:

            Debug.WriteLine($"[Client] {Service.Client}");
            var redeems2 = await EmergencyTask.API.Client.Redeem.Where(new EmergencyTask.API.ER.Redeem()
            {
                idrecompensas = 3,
                idusuario = Service.Client
            });

            if (redeems2 == null || redeems2.Count() == 0)
            {
                var redeem = await EmergencyTask.API.Client.Redeem.Add(new EmergencyTask.API.ER.Redeem()
                {
                    idrecompensas = 2,
                    idusuario = Service.Tasker
                });

                goto _reddems2;
            }

            foreach (var redeem in redeems2)
            {
                if (redeem.reclamada == 0 && redeem.realizado <= redeem.variable)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    dic.Add("realizado", (redeem.realizado + 1).ToString());
                    await EmergencyTask.API.Client.Redeem.Update(redeem.id, dic);
                    Debug.WriteLine($"[redeem] {redeem.idusuario} | {redeem.realizado} | {redeem.referencia}");
                }
            }

            IsWorkCompleted = true;
            CanFinishWork = false;
            IsBusy = false;
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

            await SocketIO?.Send(ActivityChannel, new Dictionary<string, string>
            {
                { "idestadoservicio", ((int) EstadoServicio.TrabajoTerminado).ToString() }
            });
            Debug.WriteLine($"[FinishService]");
            return true;
        }

        /// <summary>
        /// Agrega un hito
        /// </summary>
        /// <param name="stripe"></param>
        /// <param name="faltante"></param>
        /// <param name="customerid"></param>
        /// <param name="description"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<bool> AddHito(API.Stripe stripe, double faltante, string customerid, string description, string email)
        {
            var date = await Client.GetDate();
            var now = date.Value;
            var costincentavos = faltante * 100D;
            long.TryParse(costincentavos.ToString(), out long stripecost);
            if (stripecost == 0)
            {
                Toast(AppResource.ErrorServer);
                return false;
            }
            var charge = await stripe.CreateCharge((long) costincentavos, "usd", customerid, description, email, false);
            var ispreauthorized = string.IsNullOrEmpty(charge);
            var fecha = now.ToMySqlDateTimeFormat();
            var hito = GetHito(faltante, description, Service.Client, Service.Tasker, Service.Id, charge, fecha, fecha, ispreauthorized, false);
            if (hito == null) return false;
            var hitoadd = await InsertHito(hito);
            return hitoadd != null && hitoadd.id > 0;
        }

        /// <summary>
        /// Devuelve la configuracion de un hito segun la configuracion del formulario
        /// </summary>
        /// <param name="charge"></param>
        /// <param name="fechaautorizacion"></param>
        /// <param name="fechaliberacion"></param>
        /// <returns></returns>
        private Hito GetHito(double cantidad, string description, int idcliente, int idtrabajador, int idsolicitudservicio, string charge = "", string fechaautorizacion = "", string fechaliberacion = "", bool preautorizado = false, bool liberado = false)
        {
            var status = (int)HitoStatus.Created;
            if (!preautorizado)
                status = (int)HitoStatus.Created;
            else if (liberado)
                status = (int)HitoStatus.ReleaseFunds;
            else
                status = (int)HitoStatus.AuthorizedFunds;
            return new Hito
            {
                cantidademergencytasker = cantidad / 2,
                cantidadtrabajador = cantidad / 2,
                costofinal = cantidad,
                extras = 1,
                estado = status,
                cantidad = cantidad,
                descripcion = description,
                cliente = idcliente,
                trabajador = idtrabajador,
                idsolicitudservicio = idsolicitudservicio,
                chargeid = charge,
                fechadeautorizacion = fechaautorizacion,
                fechadeliberacion = fechaliberacion
            };
        }

        /// <summary>
        /// Inserta un hito a la base de datos
        /// </summary>
        /// <param name="hito"></param>
        /// <returns></returns>
        private async Task<Hito> InsertHito(Hito hito)
        {
            var hitoinweb = await Client.Hito.Add(hito);
            if (hitoinweb == null || hitoinweb.id <= 0) return null;
            return hitoinweb;
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

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
                StopSocket();
            else 
                StartSocket();
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
            else if (e.Channel == CostChannel)
            {
                SetCost(data);
            }
            else if (e.Channel == TimeChannel)
            {
                SetTime(data);
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
            if (!IsClient) return;
            if (!data.ContainsKey("costo")) return;
            if (App.Perfil == Perfil.Tasker) return;
            CostoTrabajo = data["costo"];
        }

        /// <summary>
        /// Asigna los datos de la actividad [estado de servicio]
        /// </summary>
        /// <param name="data"></param>
        private void SetActivity(Dictionary<string, string> data)
        {
            if (Service == null) return;
            if (!data.ContainsKey("idestadoservicio")) return;
            var idestadoservicio = int.Parse(data["idestadoservicio"]);
            IsWorkCompleted = idestadoservicio == (int)EstadoServicio.TrabajoTerminado;
        }
        #endregion

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
    }
}