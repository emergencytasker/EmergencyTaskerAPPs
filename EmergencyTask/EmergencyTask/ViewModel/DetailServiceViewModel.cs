using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Extensions;
using EmergencyTask.ViewModel.Validators;
using EmergencyTask.Strings;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using EmergencyTask.Views.Rating;
using Plugin.Net.Socket;

namespace EmergencyTask.ViewModel
{
    public class DetailServiceViewModel : ViewModelBase
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

        #region BindableProperty TapTimeList
        /// <summary>
        /// TapTimeList de la propiedad bindable
        /// </summary>
        private ExtendCommand taptimelist;
        public ExtendCommand TapTimeList
        {
            get { return taptimelist; }
            set { taptimelist = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IdServicio
        /// <summary>
        /// IdServicio de la propiedad bindable
        /// </summary>
        private string idservicio;
        public string IdServicio
        {
            get { return idservicio; }
            set { idservicio = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Fecha
        /// <summary>
        /// Fecha de la propiedad bindable
        /// </summary>
        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Hora
        /// <summary>
        /// Hora de la propiedad bindable
        /// </summary>
        private TimeSpan hora;
        public TimeSpan Hora
        {
            get { return hora; }
            set { hora = value; OnPropertyChanged(); }
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

        #region BindableProperty Description
        /// <summary>
        /// Description de la propiedad bindable
        /// </summary>
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Ubicacion
        /// <summary>
        /// Ubicacion de la propiedad bindable
        /// </summary>
        private string ubicacion;
        public string Ubicacion
        {
            get { return ubicacion; }
            set { ubicacion = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty CostoTime
        /// <summary>
        /// CostoTime de la propiedad bindable
        /// </summary>
        private double costotime;
        public double CostoTime
        {
            get { return costotime; }
            set { costotime = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TiempoEstimado
        /// <summary>
        /// TiempoEstimado de la propiedad bindable
        /// </summary>
        private string tiempoestimado;
        public string TiempoEstimado
        {
            get { return tiempoestimado; }
            set { tiempoestimado = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnAceptar
        /// <summary>
        /// BtnAceptar de la propiedad bindable
        /// </summary>
        private ExtendCommand btnaceptar;
        public ExtendCommand BtnAceptar
        {
            get { return btnaceptar; }
            set { btnaceptar = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property FechaText
        /// <summary>
        /// FechaText
        /// </summary>
        private string fechatext;
        public string FechaText
        {
            get { return fechatext; }
            set { fechatext = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property HoraText
        /// <summary>
        /// HoraText
        /// </summary>
        private string horatext;
        public string HoraText
        {
            get { return horatext; }
            set { horatext = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property HasSchedule
        /// <summary>
        /// HasSchedule
        /// </summary>
        private bool hasschedule;
        public bool HasSchedule
        {
            get { return hasschedule; }
            set { hasschedule = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Accesorios
        /// <summary>
        /// Accesorios
        /// </summary>
        private List<AccesorioModel> accesorios;
        public List<AccesorioModel> Accesorios
        {
            get { return accesorios; }
            set { accesorios = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Total
        /// <summary>
        /// Total
        /// </summary>
        private double total1;
        public double Total1
        {
            get { return total1; }
            set { total1 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Total2
        /// <summary>
        /// Total2
        /// </summary>
        private double total2;
        public double Total2
        {
            get { return total2; }
            set { total2 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Total
        /// <summary>
        /// Total
        /// </summary>
        private string total;
        public string Total
        {
            get { return total; }
            set { total = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property AccesoriosVisible
        /// <summary>
        /// AccesoriosVisible
        /// </summary>
        private bool accesoriosvisible;
        public bool AccesoriosVisible
        {
            get { return accesoriosvisible; }
            set { accesoriosvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property HasBeenHired
        /// <summary>
        /// HasBeenHired
        /// </summary>
        private bool hire;
        public bool Hire
        {
            get { return hire; }
            set { hire = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property HasDescription
        /// <summary>
        /// HasDescription
        /// </summary>
        private bool hasdescription;
        public bool HasDescription
        {
            get { return hasdescription; }
            set { hasdescription = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CanCancel
        /// <summary>
        /// CanCancel
        /// </summary>
        private bool cancancel;
        public bool CanCancel
        {
            get { return cancancel; }
            set { cancancel = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnCancelar
        /// <summary>
        /// BtnCancelar
        /// </summary>
        private ExtendCommand btncancelar;
        public ExtendCommand BtnCancelar
        {
            get { return btncancelar; }
            set { btncancelar = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property AccersoriosListViewHeight
        /// <summary>
        /// AccersoriosListViewHeight
        /// </summary>
        private double accesorioslistviewheight;
        public double AccersoriosListViewHeight
        {
            get { return accesorioslistviewheight; }
            set { accesorioslistviewheight = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property TrabajoTerminado
        /// <summary>
        /// TrabajoTerminado
        /// </summary>
        private bool trabajoterminado;
        public bool TrabajoTerminado
        {
            get { return trabajoterminado; }
            set { trabajoterminado = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CostoFinal
        /// <summary>
        /// CostoFinal
        /// </summary>
        private double costofinal;
        public double CostoFinal
        {
            get { return costofinal; }
            set { costofinal = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Review
        /// <summary>
        /// Revie
        /// </summary>
        private StarsReviewModel review = new StarsReviewModel { };
        public StarsReviewModel Review
        {
            get { return review; }
            set { review = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Comentario
        /// <summary>
        /// Comentario
        /// </summary>
        private string comentario;
        public string Comentario
        {
            get { return comentario; }
            set { comentario = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoToHitos
        /// <summary>
        /// GoToHitos
        /// </summary>
        private Command gotohitos;
        public Command GoToHitos
        {
            get { return gotohitos; }
            set { gotohitos = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsCanceled
        /// <summary>
        /// IsCanceled
        /// </summary>
        private bool iscanceled;
        public bool IsCanceled
        {
            get { return iscanceled; }
            set { iscanceled = value; OnPropertyChanged(); }
        }
        #endregion

        public ServiceModel Service { get; set; }
        public CandidateModel Candidate { get; set; }
        public int IdRequestService { get; set; }
        public Requestservice CurrentService { get; set; }
        public IExecuteValidator[] Validators => new IExecuteValidator[] { new InternetValidator(), new UserValidator() };
        public Xamarin.Forms.GoogleMaps.Map Mapa { get; set; }
        public StarsReview Stars { get; set; }

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

        public DetailServiceViewModel(ServiceModel service, CandidateModel candidate)
        {
            Service = service;
            Candidate = candidate;
            Hire = true;
            CanCancel = false;
            IsClient = App.Perfil == Perfil.Client;
        }

        public DetailServiceViewModel(int idrequestservice)
        {
            IdRequestService = idrequestservice;
        }

        private async Task FromRequestService(int idrequestservice)
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            if (IdRequestService == 0) return;
            IsBusy = true;
            var requestservice = await Client.Requestservice.Get(idrequestservice);
            if (requestservice == null) return;
            CandidateModel candidate = null;
            switch (usuario.Perfil)
            {
                case Perfil.Client:
                    var trabajador = await requestservice.GetTrabajador();
                    if (trabajador == null) return;
                    candidate = new CandidateModel(trabajador, new Service(
                        ));
                    break;
                case Perfil.Tasker:
                    var cliente = await requestservice.GetCliente();
                    if (cliente == null) return;
                    candidate = new CandidateModel(cliente, new Service(requestservice));
                    break;
            }
            Service = new ServiceModel(requestservice);
            Candidate = candidate;

            CanCancel = requestservice.idestadoservicio < (int) EstadoServicio.EnCaminoADomicilio && usuario.Perfil == Perfil.Client;
            if (requestservice.idestadoservicio == (int)EstadoServicio.Cancelado) CanCancel = false;

            BtnCancelar = new ExtendCommand(BtnCancelar_Clicked, Validators);
            TapTimeList = new ExtendCommand(TapTimeList_Clicked, Validators);

            if (requestservice.idestadoservicio >= (int)EstadoServicio.TrabajoTerminado)
            {
                TrabajoTerminado = true;
                await ShowFinalCost();
                await ShowReview();
            }

            IsCanceled = requestservice.idestadoservicio == (int)EstadoServicio.Cancelado;

            Hire = false;
            IsBusy = false;
        }

        /// <summary>
        /// Programar Logica
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void TapTimeList_Clicked(object obj, IExecuteValidator[] validators)
        {
            if (Service == null) return;
            await Navigation.PushAsync(new TimeWorkListPage
            {
                BindingContext = new TimeWorkListViewModel(Service.Id)
            });
        }

        private async Task ShowReview()
        {
            if (Service == null) return;
            var me = Usuario.GetUserLogin();
            if (me == null) return;
            var currentreview = (await Client.Review.Where(new Review
            {
                idperfil = me.idperfil,
                idusuario = me.id,
                idsolicitudservicio = Service.Id
            })).FirstOrDefault();
            if (currentreview == null) return;
            Stars.Value = currentreview.calificacion;
            Comentario = currentreview.comentario;
        }

        private async Task ShowFinalCost()
        {
            if (Service == null) return;
            
            var hitos = await Client.Hito.Where(new Hito
            {
                idsolicitudservicio = Service.Id,
                estado = (int) HitoStatus.ReleaseFunds
            });

            try
            {
                if (App.Perfil == Perfil.Client)
                    CostoFinal = hitos.Sum(h => h.costofinal);
                else
                    CostoFinal = hitos.Sum(h => h.cantidadtrabajador ?? 0);
            }
            catch { }

            GoToHitos = new Command(GoToHitos_Clicked);
        }

        private async void GoToHitos_Clicked(object obj)
        {
            if (Service == null) return;
            await Navigation.PushAsync(new HitoListPage
            {
                BindingContext = new HitoListViewModel(Service.Id)
            });
        }

        private async void BtnCancelar_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (Service == null) return;
            if (IsBusy) return;

            IsBusy = true;

            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Toast(AppResource.SinInternet);
                IsBusy = false;
                return;
            }

            var date = await Client.GetDate();
            if (date == null)
            {
                Toast(AppResource.IntentaMasTarde);
                IsBusy = false;
                return;
            }

            var currentservice = await Client.Requestservice.Get(Service.Id);
            if(currentservice == null)
            {
                Toast(AppResource.IntentaMasTarde);
                IsBusy = false;
                return;
            }

            if (currentservice.idestadoservicio >= (int)EstadoServicio.EnCaminoADomicilio)
            {
                IsBusy = false;
                Toast(AppResource.ServicioProgreso);
                await Load();
                return;
            }

            if (currentservice.idestadoservicio == (int)EstadoServicio.Cancelado)
            {
                IsBusy = false;
                Toast(AppResource.ServicioCancelado);
                return;
            }

            var now = date.Value;
            var elapsed = (Service.Date.Subtract(now)).TotalHours;

            bool istocanceled;
            bool payfee = false;
            double amountfee = 0;
            bool status = false;

            if (elapsed > 24)
            {
                if (!await Confirm(AppResource.SeguroCancelarServicio))
                {
                    IsBusy = false;
                    return;
                }
                istocanceled = true;
            }
            else
            {
                if (!await Confirm($"{string.Format(AppResource.ServicioAtendidoEn, Math.Round(elapsed, 0))}, {AppResource.SiCancelas} {Service.CostoPorHora}"))
                {
                    IsBusy = false;
                    return;
                }
                istocanceled = true;
                payfee = true;
                amountfee = Service.CostoPorHora;
            }

            IsBusy = false;

            if (!istocanceled)
            {
                Toast(AppResource.SolicitudNoProcesada);
                return;
            }

            await SetReasonCanceled(Service.Id, async () =>
            {
                status = await Cancel(payfee, amountfee);
                if (status && payfee) 
                    DisplayAlert(string.Format(AppResource.ReembolsoDeServicio, (Service.CostoPorHora * Service.Time) - Service.CostoPorHora, 
                        await SecureStorage.GetAsync("CardLast4")), 
                        AppResource.Aceptar);
                var socket = await SocketFactory.Instance.Resolve();
                if (socket != null)
                {
                    var action = (int)EstadoServicio.Cancelado;
                    await socket.Send("Service", new Dictionary<string, string>
                    {
                        { ServiceModel.Action, (action).ToString() },
                        { ServiceModel.IdKey, Service.Id.ToString() }
                    });
                }
                IsBusy = false;
                SetDetailPage(new CalendarPage());
            });
        }

        /// <summary>
        /// Abre el popup para cancelar una razón
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task SetReasonCanceled(int id, Action action)
        {
            await PopupNavigation.Instance.PushAsync(new CanceledReasonPopup
            {
                BindingContext = new CanceledReasonViewModel(id)
                {
                    Completed = action
                }
            });
        }

        /// <summary>
        /// Cancela un trabajo
        /// </summary>
        /// <param name="cobrartarifa"></param>
        /// <param name="tarifa"></param>
        /// <returns></returns>
        private async Task<bool> Cancel(bool cobrartarifa, double tarifa)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return false;
            if (Service == null) return false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) return false;
            var stripe = await App.GetStripeAsync();
            if (stripe == null) return false;
            var hito = await Client.Hito.Where(new Hito
            {
                trabajoterminado = 1,
                idsolicitudservicio = Service.Id
            }) ?? new List<Hito>();
            var pendingforpay = hito.FirstOrDefault(h => h.estado == (int)HitoStatus.AuthorizedFunds);
            if (pendingforpay == null) return false;
            var chargeid = pendingforpay.chargeid;
            if (!cobrartarifa) return await stripe.CancelCharge(chargeid);
            long.TryParse((tarifa * 100).ToString(), out long stripecost);
            if (stripecost <= 0) return false;
            var refund = await stripe.CaptureCharge(chargeid, stripecost);
            if (!refund) return false;
            return await Client.ChangeServiceStatus(Service.Id, EstadoServicio.Cancelado, me.id, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0);
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            await Load();
        }

        private async Task Load()
        {
            IsBusy = true;
            await FromRequestService(IdRequestService);
            LoadDataFromService();
            SetPositionAtMap();
            if (IdRequestService > 0)
            {
                IdServicio = $"{AppResource.Servicio} #{IdRequestService}";
            }
            BtnAceptar = new ExtendCommand(BtnAceptar_Clicked, new InternetValidator());
            IsBusy = false;
        }

        private void SetPositionAtMap()
        {
            if (Service == null) return;
            Mapa.Pins.Clear();
            var position = new Xamarin.Forms.GoogleMaps.Position(Service.Latitud, Service.Longitud);
            Mapa.Pins.Add(new Xamarin.Forms.GoogleMaps.Pin
            {
                Position = position,
                Address = Service.Direccion,
                Label = Service.Category + " • " + Service.SubCategory
            });
            Mapa.MoveToRegion(Xamarin.Forms.GoogleMaps.MapSpan.FromCenterAndRadius(position, Xamarin.Forms.GoogleMaps.Distance.FromKilometers(3)));
        }

        private void LoadDataFromService()
        {
            if (Candidate == null || Service == null) return; 
            Service.CostoPorHora = Candidate.Cost;
            HasSchedule = Service.HasSchedule;
            if (Service.HasSchedule)
            {
                Hora = Service.Start.TimeOfDay;
                Fecha = Service.Start;
            }
            FotoAsistente = Candidate.FotoAsistente;
            NombreAsistente = Candidate.NombreAsistente;
            Servicio = AppResource.Servicio + ": " + Service.Category;
            Tarea = AppResource.Tarea + ": " + Service.SubCategory;
            TiempoEstimado = string.Format(AppResource.RecuerdaPrecioFinal, Service.Time);
            Indicaciones = Service.Detalles;
            Description = Service.Description;
            HasDescription = !string.IsNullOrEmpty(Service.Detalles);
            Ubicacion = Service.Direccion;
            CostoTime = Candidate.Cost;
            FechaText = Fecha.ToString(AppResource.PrettyDateFormat);
            HoraText = Hora.ToString(@"hh\:mm");
            Accesorios = Service.Accesorios;
            AccesoriosVisible = Service.Accesorios.Count > 0;
            AccersoriosListViewHeight = Service.Accesorios.Count * 45;
            var subtotal = 0D;
            // Total1 = subtotal + (Service.Time / 2 * Candidate.Cost);
            Total2 = subtotal + (Service.Time * Candidate.Cost);
            Total = $"{AppResource.ContratarServicio}";
        }

        public void SetMapa(Xamarin.Forms.GoogleMaps.Map mapa)
        {
            Mapa = mapa;
        }

        private async void BtnAceptar_Clicked(object obj, IExecuteValidator[] validators)
        {
            var usuario = Usuario.GetUserLogin();
            if(usuario == null)
            {
                if (await RequestUser()) return;
                Toast(AppResource.SinSesionIniciada);
                return;
            }

            IsBusy = true;

            var date = await Client.GetDate();
            if (date == null)
            {
                Toast(AppResource.SinConfigurarHora);
                return;
            }
            var now = date.Value;

            if(Candidate == null)
            {
                Toast(AppResource.SinAgendarServicio);
                return;
            }

            if (Service == null)
            {
                Toast(AppResource.SinAgendarServicio);
                return;
            }

            var stripe = await App.GetStripeAsync();
            if (stripe == null)
            {
                Toast(AppResource.SinAgendarServicio);
                IsBusy = false;
                return;
            }

            if (!await usuario.HasPaymethod())
            {
                if (await Confirm(AppResource.ConfigurarFormaPago))
                {
                    await Navigation.PushPopupAsync(new AddPaymentPopUp());
                }
                IsBusy = false;
                return;
            }

            var customerid = await usuario.GetCustomerId();
            if (string.IsNullOrEmpty(customerid))
            {
                Toast(AppResource.SinAgendarServicio);
                IsBusy = false;
                return;
            }

            var subcategoria = await Client.Subcategory.Get(Service.IdSubcategory);
            if(subcategoria == null)
            {
                Toast(AppResource.NoAgendarServicio);
                return;
            }

            var cost = subcategoria.GetSchedules().Max(s => s.costo);
            if (cost <= 0)
            {
                Toast(AppResource.NoAgendarServicio);
                return;
            }

            var servicehours = Service.Time;
            var servicecost = cost * Service.Time;
            var servicedescription = $"{Service.Category} • {Service.SubCategory}";

            var startservicedate = Service.HasSchedule ? Service.Date.ToMySqlDateTimeFormat() : now.ToMySqlDateTimeFormat();
            var endservicedate = (Service.HasSchedule ? Service.Date.AddHours(Service.Time) : now.AddHours(Service.Time)).ToMySqlDateTimeFormat();

            var costincentavos = servicecost * 100D;
            long.TryParse(costincentavos.ToString(), out long stripecost);

            if (stripecost == 0)
            {
                Toast(AppResource.NoAgendarServicio);
                IsBusy = false;
                return;
            }

            var chargeid = await stripe.CreateCharge(stripecost, "usd", customerid, servicedescription, usuario.email);
            if (string.IsNullOrEmpty(chargeid))
            {
                Toast(AppResource.NoValidoFormaPago);
                IsBusy = false;
                return;
            }

            WaitForAcceptingPopup popup = null;
            if (!Service.HasSchedule)
            {
                popup = new WaitForAcceptingPopup();
                await Navigation.PushPopupAsync(popup);
            }

            CurrentService = await Client.Requestservice.Add(new Requestservice
            {
                trabajador = Candidate.Id,
                categoria = Service.Category,
                cliente = usuario.id,
                costoporhora = Service.CostoPorHora,
                tiemposolicitado = Service.Time,
                descripcion = Service.Description,
                detalles = Service.Detalles,
                direccion = Service.Direccion,
                tienehorario = Service.HasSchedule ? 1 : 0,
                idcategoria = Service.IdCategoria,
                idsubcategoria = Service.IdSubcategory,
                idservicio = Candidate.ServiceId,
                idestadoservicio = (int) EstadoServicio.Pendiente,
                longitud = Service.Longitud,
                latitud = Service.Latitud,
                subcategoria = Service.SubCategory,
                fechadeservicio = startservicedate,
                fechainicio = startservicedate,
                fechafin = endservicedate,
                tieneaccesorios = (Service.Accesorios != null && Service.Accesorios.Count > 0) ? 1 : 0
            });

            if (CurrentService == null || CurrentService.id <= 0)
            {
                if (popup != null)
                {
                    try { await Navigation.PopPopupAsync(); } catch { }
                }
                Toast(AppResource.IntentarMasTarde);
                IsBusy = false;
                return;
            }

            var socket = await SocketFactory.Instance.Resolve();
            await socket.Send($"NewServiceChannel-{Candidate.Id}", new Dictionary<string, string>
            {
                { "idsolicitudservicio", CurrentService.id.ToString() }
            });

            var hito = await Client.Hito.Add(new Hito
            {
                cantidad = servicecost,
                cliente = usuario.id,
                trabajador = Candidate.Id,
                descripcion = CurrentService.categoria + " • " + CurrentService.subcategoria,
                idsolicitudservicio = CurrentService.id,
                chargeid = chargeid,
                estado = (int) HitoStatus.AuthorizedFunds,
                trabajoterminado = 1,
                fechadeautorizacion = now.ToMySqlDateTimeFormat()
            });

            if (Service.Accesorios != null)
            {
                var accesorios = Service.Accesorios.Select(a =>
                {
                    return new Accessory
                    {
                        costo = 0,
                        idsolicitudservicio = CurrentService.id,
                        cantidad = a.Cantidad,
                        nombre = a.Nombre
                    };
                }).ToList();

                if (accesorios.Count > 0)
                {
                    var accesories = await Client.Accessory.Add(accesorios) ?? new List<Accessory>();
                    if (accesorios.Count == accesories.Count())
                    {
                        Toast(AppResource.SinAccesorios);
                    }
                }
            }

            await Client.ChangeServiceStatus(CurrentService.id, EstadoServicio.Pendiente, usuario.id, Latitud, Longitud, Latitud != 0D && Longitud != 0D ? 1 : 0);

            if (!Service.HasSchedule)
            {
                if (popup != null)
                {
                    try { await Navigation.PopPopupAsync(); } catch { }
                }
                ReturnToHomePage(true);
                IsBusy = false;
                return;
            }

            if (popup != null && PopupNavigation.Instance.PopupStack.Contains(popup))
                try { await PopupNavigation.Instance.RemovePageAsync(popup); } catch { }

            SecureStorage.RemoveAll();
            ReturnToHomePage(false);
            Toast(AppResource.CitaAgendada);
        }

        private async Task<bool> RequestUser()
        {
            var action = await ActionSheet("Inicia sesión o registrate para continuar", "Cancelar", "Registrarse", "Iniciar sesion");
            if (action == "Registrarse") { await SignUp(); return true; }
            if (action == "Iniciar sesion") { await SignIn(); return true; }
            return false;
        }

        private async Task SignIn()
        {
            await StorageService();
            await Navigation.PushAsync(new LoginPage());
        }

        private async Task SignUp()
        {
            await StorageService();
            await Navigation.PushAsync(new RegisterPage());
        }

        private async Task StorageService()
        {
            SecureStorage.RemoveAll();
            await SecureStorage.SetAsync("pendingservice", Newtonsoft.Json.JsonConvert.SerializeObject(Service));
        }

        private void ReturnToHomePage(bool showmessage)
        {
            if (CurrentService == null) return;
            CurrentService = null;
            if (showmessage)
            {
                Toast(AppResource.ServicioEnProceso);
            }
            SetDetailPage(new CalendarPage());
        }

        public void SetStars(StarsReview stars)
        {
            Stars = stars;
        }
    }
}
