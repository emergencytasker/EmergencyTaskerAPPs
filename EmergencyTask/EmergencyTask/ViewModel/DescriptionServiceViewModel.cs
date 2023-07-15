using System;
using System.Collections.Generic;
using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class DescriptionServiceViewModel : ViewModelBase
    {

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

        #region BindableProperty Btn12h
        /// <summary>
        /// Btn12h de la propiedad bindable
        /// </summary>
        private Command btn12h;
        public Command Btn12h
        {
            get { return btn12h; }
            set { btn12h = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Btn24h
        /// <summary>
        /// Btn24h de la propiedad bindable
        /// </summary>
        private Command btn24h;
        public Command Btn24h
        {
            get { return btn24h; }
            set { btn24h = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Btnmore4h
        /// <summary>
        /// Btnmore4h de la propiedad bindable
        /// </summary>
        private Command btnmore4h;
        public Command Btnmore4h
        {
            get { return btnmore4h; }
            set { btnmore4h = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnConfirmar
        /// <summary>
        /// BtnConfirmar de la propiedad bindable
        /// </summary>
        private ExtendCommand btnconfirmar;
        public ExtendCommand BtnConfirmar
        {
            get { return btnconfirmar; }
            set { btnconfirmar = value; OnPropertyChanged(); }
        }

        public ServiceModel Service { get; set; }
        #endregion

        #region Notified Property FechaOK
        /// <summary>
        /// FechaOK
        /// </summary>
        private bool fechaok;
        public bool FechaOK
        {
            get { return fechaok; }
            set { fechaok = value; OnPropertyChanged(); }
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

        #region Notified Property Color12h
        /// <summary>
        /// Color12h
        /// </summary>
        private Color color12h = Color.White;
        public Color Color12h
        {
            get { return color12h; }
            set { color12h = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Color24h
        /// <summary>
        /// Color24h
        /// </summary>
        private Color color24h = Color.White;
        public Color Color24h
        {
            get { return color24h; }
            set { color24h = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Colormore4
        /// <summary>
        /// Colormore4
        /// </summary>
        private Color colormore4 = Color.White;
        public Color Colormore4
        {
            get { return colormore4; }
            set { colormore4 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Text12h
        /// <summary>
        /// Text12h
        /// </summary>
        private Color text12h = Color.Black;
        public Color Text12h
        {
            get { return text12h; }
            set { text12h = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Text24h
        /// <summary>
        /// Text24h
        /// </summary>
        private Color text24h = Color.Black;
        public Color Text24h
        {
            get { return text24h; }
            set { text24h = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Textmore4
        /// <summary>
        /// Textmore4
        /// </summary>
        private Color textmore4 = Color.Black;
        public Color Textmore4
        {
            get { return textmore4; }
            set { textmore4 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CanChangeHasSchedule
        /// <summary>
        /// CanChangeHasSchedule
        /// </summary>
        private bool canchangehasschedule = true;
        public bool CanChangeHasSchedule
        {
            get { return canchangehasschedule; }
            set { canchangehasschedule = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Hours
        /// <summary>
        /// Hours
        /// </summary>
        private IEnumerable<EstimateTime> hours;
        public IEnumerable<EstimateTime> Hours
        {
            get { return hours; }
            set { hours = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property MaximumDate
        /// <summary>
        /// MaximumDate
        /// </summary>
        private DateTime maximumDate = DateTime.Now;
        public DateTime MaximumDate
        {
            get { return maximumDate; }
            set { maximumDate = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property MinimumDate
        /// <summary>
        /// MinimumDate
        /// </summary>
        private DateTime minimumDate = DateTime.Now;
        public DateTime MinimumDate
        {
            get { return minimumDate; }
            set { minimumDate = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Hour
        /// <summary>
        /// Hour
        /// </summary>
        private EstimateTime hour;
        public EstimateTime Hour
        {
            get { return hour; }
            set { hour = value; OnPropertyChanged(); }
        }
        #endregion

        public DateTime LimitDate { get; set; } = DateTime.Now;

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            var estimatetimes = new List<EstimateTime>();
            for (int i = 1; i <= 8; i++)
            {
                estimatetimes.Add(new EstimateTime
                {
                    Hour = i,
                    Description = string.Format(AppResource.Horas, i) 
                });
            }
            Hours = estimatetimes;
            BtnConfirmar = new ExtendCommand(BtnConfirmar_Clicked, new InternetValidator());
            var date = await Client.GetDate();
            if (date == null)
            {
                Toast(AppResource.NoConfiguracionHora);
                return;
            }
            var now = date.Value;
            MinimumDate = now;
            MaximumDate = now.AddDays(5);
            Fecha = now;
            Hora = now.TimeOfDay;
            if (Service == null) Service = new ServiceModel();

            var usuario = Usuario.GetUserLogin();
            if (usuario != null)
            {
                var currentservice = await Client.GetCurrentService(usuario.Perfil, usuario.id);
                if (currentservice == null) return;
                SetTimeValidations(currentservice);
            }
        }

        private void SetTimeValidations(Requestservice currentservice)
        {
            LimitDate = currentservice.fechafin.FromMySqlDateTimeFormat();
            HasSchedule = true;
            CanChangeHasSchedule = false;
        }

        private async void BtnConfirmar_Clicked(object obj, IExecuteValidator[] validators)
        {
            IsBusy = true;

            if (HasSchedule)
            {
                var dateservice = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, Hora.Hours, Hora.Minutes, Hora.Seconds);
                if (dateservice <= LimitDate)
                {
                    if (!CanChangeHasSchedule)
                    {
                        Toast($"{AppResource.NoPuedesAgendar} {LimitDate}");
                        IsBusy = false;
                        return;
                    }
                    Toast(AppResource.FechaNoValida);
                    IsBusy = false;
                    return;
                }

                Service.Date = dateservice;
            }
            else
            {
                var date = await Client.GetDate();
                if (date == null)
                {
                    Toast(AppResource.NoPodemosContinuar);
                    IsBusy = false;
                    return;
                }
                var now = date.Value;
                Service.Date = now;
            }

            if(Hour == null)
            {
                Toast(AppResource.ElegirTiempoEstimado);
                IsBusy = false;
                return;
            }

            if (string.IsNullOrEmpty(Description))
            {
                Toast(AppResource.IngresaDescripcion);
                IsBusy = false;
                return;
            }

            if(Description.Length < 15)
            {
                Toast(AppResource.MensajeMinimo);
                IsBusy = false;
                return;
            }

            Service.HasSchedule = HasSchedule;
            Service.Description = Description;
            Service.Start = Service.Date;
            Service.Time = Hour.Hour;
            Service.End = Service.Date.AddHours(Service.Time);

            if (await Confirm(AppResource.MaterialExtra))
            {
                await Navigation.PushAsync(new AccesoriesListPage
                {
                    BindingContext = new AccesoriesListViewModel(Service)
                });
            }
            else
            {
                await Navigation.PushAsync(new CandidateListPage
                {
                    BindingContext = new CandidateListViewModel(Service)
                });
            }

            IsBusy = false;
        }
    }
}
