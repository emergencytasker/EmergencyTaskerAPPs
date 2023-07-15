using EmergencyTask.API;
using EmergencyTask.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using EmergencyTask.Strings;
using Rg.Plugins.Popup.Extensions;
using System;

namespace EmergencyTask.ViewModel
{
    public class CandidateListViewModel : ViewModelBase
    {

        #region BindableProperty Candidate
        /// <summary>
        /// Candidate de la propiedad bindable
        /// </summary>
        private CandidateModel candidate;
        public CandidateModel Candidate
        {
            get { return candidate; }
            set { candidate = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Candidates
        /// <summary>
        /// Candidates de la propiedad bindable
        /// </summary>
        private ObservableCollection<CandidateModel> candidates;
        public ObservableCollection<CandidateModel> Candidates
        {
            get { return candidates; }
            set { candidates = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapInfoCosto
        /// <summary>
        /// TapInfoCosto de la propiedad bindable
        /// </summary>
        private Command tapinfocosto;
        public Command TapInfoCosto
        {
            get { return tapinfocosto; }
            set { tapinfocosto = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Filter
        /// <summary>
        /// Filter
        /// </summary>
        private Command filter;
        public Command Filter
        {
            get { return filter; }
            set { filter = value; OnPropertyChanged(); }
        }
        #endregion

        public List<CandidateModel> Source { get; set; }
        public ServiceModel Service { get; set; }
        private IExecuteValidator UserValidator { get; set; }
        private IExecuteValidator InternetValidator { get; set; }

        public CandidateListViewModel(ServiceModel service)
        {
            Service = service;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;
            UserValidator = new UserValidator();
            InternetValidator = new InternetValidator();
            Source = new List<CandidateModel>();
            Filter = new Command(Filter_Command);
            var taskers = await Client.ListTaskersByAvailability(Service.Latitud, Service.Longitud, Service.IdCategoria, Service.IdSubcategory, Service.Date, TimeSpan.FromHours(Service.Time));
            Source = new List<CandidateModel>(taskers.Select(t => new CandidateModel(t)
            {
                BtnContratar = new ExtendCommand(BtnContratar_Clicked, UserValidator, InternetValidator),
                TapMessage = new ExtendCommand(TapMessage_Clicked, UserValidator, InternetValidator),
                TapProfile = new ExtendCommand(TapProfile_Clicked, UserValidator, InternetValidator),
                TapInfoCosto = new Command(TapInfoCosto_Clicked)
            }));
            SetSource(Source);
            IsBusy = false;
        }

        /// <summary>
        /// Programar Funcionalidad
        /// </summary>
        /// <param name="obj"></param>
        private async void TapInfoCosto_Clicked(object obj)
        {
            await Navigation.PushPopupAsync(new CostPerHourPopup());
        }

        public int GetDay(DayOfWeek item)
        {
            var dia = 0;
            switch (item)
            {
                case DayOfWeek.Friday:
                    dia = 6;
                    break;
                case DayOfWeek.Monday:
                    dia = 2;
                    break;
                case DayOfWeek.Saturday:
                    dia = 7;
                    break;
                case DayOfWeek.Sunday:
                    dia = 1;
                    break;
                case DayOfWeek.Thursday:
                    dia = 5;
                    break;
                case DayOfWeek.Tuesday:
                    dia = 3;
                    break;
                case DayOfWeek.Wednesday:
                    dia = 4;
                    break;
                default:
                    break;
            }
            return dia;
        }

        private void SetSource(IEnumerable<CandidateModel> source)
        {
            if (source == null || source.Count() == 0) return;
            if (Candidates == null) Candidates = new ObservableCollection<CandidateModel>();
            Candidates.Clear();
            foreach (var item in source)
                Candidates.Add(item);
        }

        private async void Filter_Command(object obj)
        {
            await Navigation.PushPopupAsync(new FilterPage(async (filterresult) =>
            {
                await Navigation.PopPopupAsync();
                IsBusy = true;
                IEnumerable<CandidateModel> candidates = Source.ToList();

                if(filterresult.Calificacion && filterresult.Tareas)
                    candidates = Source.OrderByDescending(o => o.Calificacion).ThenByDescending(o => o.Tareas);
                else if(filterresult.Calificacion && !filterresult.Tareas)
                    candidates = Source.OrderByDescending(o => o.Calificacion);
                else if(!filterresult.Calificacion && filterresult.Tareas)
                    candidates = Source.OrderByDescending(o => o.Tareas);

                if (filterresult.Distancia > 0)
                    candidates = candidates.Where(o => o.Distancia <= filterresult.Distancia);

                SetSource(candidates);
                IsBusy = false;
            }));
        }

        private async void TapProfile_Clicked(object obj, IExecuteValidator[] validators)
        {
            if (!(obj is CandidateModel model)) return;
            await Navigation.PushAsync(new ProfileTaskerPage
            {
                BindingContext = new ProfileTaskerViewModel(model.IdUsuario)
            });
        }

        private async void TapMessage_Clicked(object obj, IExecuteValidator[] validators)
        {
            if (!(obj is CandidateModel model)) return;
            if (!(validators.TryGetComparator(out Usuario me))) return;

            IsBusy = true;

            await Navigation.PushAsync(new ChatPage(me.id, model.Id)
            {
                Title = string.Format(AppResource.ChatCon, model.NombreAsistente)
            });

            IsBusy = false;
        }

        private async void BtnContratar_Clicked(object obj, IExecuteValidator[] validators)
        {
            if (!(obj is CandidateModel model)) return;
            IsBusy = true;
            await Navigation.PushAsync(new DetailServicePage
            {
                BindingContext = new DetailServiceViewModel(Service, model)
            });
            IsBusy = false;
        }
    }
}