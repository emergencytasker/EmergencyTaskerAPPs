using System;
using System.Collections.Generic;
using System.Linq;
using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Business;
using LightForms.Commands;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace EmergencyTask.ViewModel
{
    public class RequestHoursViewModel : ViewModelBase
    {

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

        #region Notified Property BtnRequest
        /// <summary>
        /// BtnRequest
        /// </summary>
        private Command btnrequest;
        public Command BtnRequest
        {
            get { return btnrequest; }
            set { btnrequest = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Description
        /// <summary>
        /// Description
        /// </summary>
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CalculateCost
        /// <summary>
        /// CalculateCost
        /// </summary>
        private ICommand calculatecost;
        public ICommand CalculateCost
        {
            get { return calculatecost; }
            set { calculatecost = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property EstimateCost
        /// <summary>
        /// EstimateCost
        /// </summary>
        private double estimatecost;
        public double EstimateCost
        {
            get { return estimatecost; }
            set { estimatecost = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnClose
        /// <summary>
        /// BtnClose
        /// </summary>
        private ICommand btnclose;
        public ICommand BtnClose
        {
            get { return btnclose; }
            set { btnclose = value; OnPropertyChanged(); }
        }
        #endregion

        public int IdSolicitudservicio { get; set; }
        public int IdTrabajador { get; set; }
        public int IdCliente { get; set; }
        public Action Success { get; set; }

        public RequestHoursViewModel(int idsolicitudservicio, int idtrabajador, int idcliente)
        {
            IdSolicitudservicio = idsolicitudservicio;
            IdTrabajador = idtrabajador;
            IdCliente = idcliente;
        }

        public override void OnAppearing(Xamarin.Forms.Page page)
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
            BtnClose = new Command(BtnClose_Clicked);
            BtnRequest = new Command(BtnRequest_Clicked, (obj) => IsValid());
            CalculateCost = new Command(CalculateCost_Command);
        }

        private async void BtnClose_Clicked(object obj)
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch { }
        }

        private void CalculateCost_Command(object obj)
        {
            BtnRequest?.RaiseCanExecuteChanged();
            if (!IsValid()) return;
            var hours = Hour.Hour;
            var report = new CostCalculator().Calculate(DateTime.Now, DateTime.Now.AddHours(hours), 100, 200);
            EstimateCost = report.Max(r => r.Tarifa) * hours;
        }

        private bool IsValid() => (Hour?.Hour ?? 0) > 0;

        private async void BtnRequest_Clicked(object obj)
        {
            IsBusy = true;
            var hours = Hour.Hour;
            var now = DateTime.Now;
            var end = DateTime.Now.AddHours(hours);
            var report = new CostCalculator().Calculate(now, end, 100, 200);
            var finalcost = report.Max(r => r.Tarifa) * hours;

            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert(AppResource.SinInternet, AppResource.Aceptar);
                IsBusy = false;
                return;
            }

            var servicio = await Client.Requestservice.Update(IdSolicitudservicio, new Dictionary<string, string>
            {
                { nameof(Requestservice.costoporhora), finalcost.ToString() },
                { nameof(Requestservice.tiemposolicitado), hours.ToString() }
            });

            if (servicio == null || servicio.costoporhora != finalcost || servicio.tiemposolicitado != hours)
            {
                DisplayAlert(AppResource.ErrorServer, AppResource.Aceptar);
                IsBusy = false;
                return;
            }

            var hito = await Client.Hito.Add(new Hito
            {
                cantidad = finalcost,
                cliente = IdCliente,
                trabajador = IdTrabajador,
                descripcion = description,
                idsolicitudservicio = IdSolicitudservicio,
                estado = (int) HitoStatus.Created,
                trabajoterminado = 1,
                fechadeautorizacion = DateTime.Now.ToMySqlDateTimeFormat(),
                extras = 1
            });

            if(hito == null || hito.id <= 0)
            {
                DisplayAlert(AppResource.ErrorServer, AppResource.Aceptar);
                IsBusy = false;
                return;
            }

            await Client.Time.Add(new Time
            {
                chargeid = string.Empty,
                costo = finalcost,
                eliminado = 0,
                finalizado = 1,
                idsolicitudservicio = IdSolicitudservicio,
                trabajador = IdTrabajador,
                tiempo = TimeSpan.FromSeconds(hours).ToString(),
                fechainicio = now.ToMySqlDateTimeFormat(),
                fechafin = now.ToMySqlDateTimeFormat()
            });

            await Client.SendNotification(IdCliente, AppResource.HorasExtra, string.Format(AppResource.TaskerSolicitaHorasExtra, hours), hito.id, (int)NotificationActions.PayRequest);

            DisplayAlert(string.Format(AppResource.HorasExtraSolicitasCorrectamente, hours), "Aceptar");

            try { await PopupNavigation.Instance.PopAsync(); } catch { }

            Success?.Invoke();

            IsBusy = false;
        }
    }
}
