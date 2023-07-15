using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using Plugin.UI.Xaml.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using EmergencyTask.API.Enum;
using EmergencyTask.Strings;
using System.Windows.Input;
using System.Diagnostics;

namespace EmergencyTask.ViewModel
{
    public class CalendarViewModel : ViewModelBase, ICalendarBehavior
    {

        #region Notified Property SelectedEvent
        /// <summary>
        /// Event
        /// </summary>
        private Command<IEvent> selectedevent;
        public Command<IEvent> SelectedEvent
        {
            get { return selectedevent; }
            set { selectedevent = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Event
        /// <summary>
        /// Event
        /// </summary>
        private IEvent ievent;
        public IEvent Event
        {
            get { return ievent; }
            set { ievent = value; OnPropertyChanged(); if (value != null) { SelectedEventClicked(value); } }
        }
        #endregion

        #region Notified Property Events
        /// <summary>
        /// Events
        /// </summary>
        private ObservableCollection<IEvent> events;
        public ObservableCollection<IEvent> Events
        {
            get { return events; }
            set { events = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property SelectedDay
        /// <summary>
        /// SelectedDay
        /// </summary>
        private Command<DateTime> selecteddate;
        public Command<DateTime> SelectedDate
        {
            get { return selecteddate; }
            set { selecteddate = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property SelectedSegment
        /// <summary>
        /// SelectedSegment
        /// </summary>
        private Command selectedsegment;
        public Command SelectedSegment
        {
            get { return selectedsegment; }
            set { selectedsegment = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsCalendarVisible
        /// <summary>
        /// IsCalendarVisible
        /// </summary>
        private bool iscalendarvisible;
        public bool IsCalendarVisible
        {
            get { return iscalendarvisible; }
            set { iscalendarvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsListVisible
        /// <summary>
        /// IsListVisible
        /// </summary>
        private bool islistvisible;
        public bool IsListVisible
        {
            get { return islistvisible; }
            set { islistvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoSchedules
        /// <summary>
        /// GoSchedules
        /// </summary>
        private ICommand goschedules;
        public ICommand GoSchedules
        {
            get { return goschedules; }
            set { goschedules = value; OnPropertyChanged(); }
        }
        #endregion

        public IEnumerable<Requestservice> Services { get; set; }

        public CalendarViewModel()
        {
            Events = new ObservableCollection<IEvent>(); // dont move!!
        }

        /// <summary>
        /// Opcion de vista seleccionada
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedSegmentClicked(object obj)
        {
            if (!(obj is int opcion)) return;
            switch (opcion)
            {
                case 0:
                    IsCalendarVisible = true;
                    IsListVisible = false;
                    break;

                case 1:
                    IsListVisible = true;
                    IsCalendarVisible = false;
                    break;
            }
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;
            IsCalendarVisible = true;
            if (!await EnsureApi())
            {
                IsBusy = false;
                return;
            }
            LoadEvents();
            SelectedEvent = new Command<IEvent>(SelectedEventClicked);
            SelectedDate = new Command<DateTime>(SelectDateClicked);
            SelectedSegment = new Command(SelectedSegmentClicked);
            GoSchedules = new Command(GoSchedulesClicked);
            IsBusy = false;
        }

        private async void GoSchedulesClicked(object obj)
        {
            await Navigation.PushAsync(new ScheduleListPage());
        }

        /// <summary>
        /// Carga los eventos
        /// </summary>
        private async void LoadEvents()
        {
            if (Services == null) return;
            if (Events == null) return;
            IsBusy = true;
            var usuario = Usuario.GetUserLogin();
            if(usuario != null)
            {
                var idlenguaje = await Client.GetLanguage(usuario.lenguaje);
                var categories = await Client.GetCategoriesByLanguage(idlenguaje) ?? new List<Categorylanguage>();
                var subcategories = await Client.GetSubCategoriesByLanguage(idlenguaje) ?? new List<Subcategorylanguage>();
                foreach (var service in Services)
                {
                    Events.Add(new EmergencyEvent(service, categories, subcategories));
                }
            }
            IsBusy = false;
        }

        /// <summary>
        /// Verifica que se hayan bajado todos los datos del server
        /// </summary>
        /// <returns></returns>
        private async Task<bool> EnsureApi()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return false;
            var requestservice = usuario.Perfil == Perfil.Client ? new Requestservice
            {
                cliente = usuario.id
            } : new Requestservice
            {
                trabajador = usuario.id
            };
            Services = await Client.Requestservice.Where(requestservice) ?? new List<Requestservice>();
            return Services.Count() > 0;
        }

        private void SelectDateClicked(DateTime obj)
        {
        }

        /// <summary>
        /// Evento seleccionado desde el calendario
        /// </summary>
        /// <param name="obj"></param>
        private async void SelectedEventClicked(IEvent obj)
        {
            if (Services == null) return;
            if (obj == null) return;

            if (IsBusy) return;
            IsBusy = true;

            if (!await Confirm(AppResource.VerDetalle))
            {
                IsBusy = false;
                return;
            }

            var requestservice = Services.FirstOrDefault(s => s.id == obj.Id);
            if (requestservice == null)
            {
                IsBusy = false;
                return;
            }

            var estadoactual = (EstadoServicio)requestservice.idestadoservicio;

            Debug.WriteLine($"[SelectedEventClicked] {estadoactual}");
            switch (estadoactual)
            {
                case EstadoServicio.Cancelado:
                    Toast(AppResource.ServicioCancelado);
                    break;

                case EstadoServicio.Pendiente:
                    await GoToServiceInfoPage(obj.Id);
                    break;

                case EstadoServicio.Finalizado:
                case EstadoServicio.Aceptado:
                case EstadoServicio.HerramientasCompradas:
                    await GoToServiceDetailPage(obj.Id);
                    break;
                case EstadoServicio.EnCaminoADomicilio:
                    await GoToServiceDetailPage(obj.Id);
                    break;
                case EstadoServicio.LlegadaADomicilio:
                    await GoToServiceDetailPage(obj.Id);
                    break;
                case EstadoServicio.TrabajoIniciado:
                    await GoToServiceDetailPage(obj.Id);
                    break;

                case EstadoServicio.TrabajoTerminado:
                    if(!await GoToReview(obj.Id))
                    {
                        await GoToEvidence(obj.Id);
                    }
                    break;

                case EstadoServicio.Calificado:
                    await GoToEvidence(obj.Id);
                    break;
            }
            IsBusy = false;
        }
    }
}