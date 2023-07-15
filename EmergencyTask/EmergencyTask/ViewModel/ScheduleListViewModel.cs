using EmergencyTask.API;
using EmergencyTask.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ScheduleListViewModel : ViewModelBase
    {

        #region BindableProperty Horarios
        /// <summary>
        /// Horarios de la propiedad bindable
        /// </summary>
        private ObservableCollection<ScheduleDayModel> horarios;
        public ObservableCollection<ScheduleDayModel> Horarios
        {
            get { return horarios; }
            set { horarios = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnEdit
        /// <summary>
        /// BtnEdit de la propiedad bindable
        /// </summary>
        private Command btnedit;
        public Command BtnEdit
        {
            get { return btnedit; }
            set { btnedit = value; OnPropertyChanged(); }
        }
        #endregion

        public ScheduleListViewModel()
        {

        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            await LoadSchedule();
        }

        private async Task LoadSchedule()
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;

            var agenda = await Client.Calendar.Where(new API.ER.Calendar
            {
                idusuario = me.id
            });

            BtnEdit = new Command(BtnEdit_Clicked);

            var list = new List<ScheduleDayModel>();
            foreach (var item in agenda.GroupBy(a => a.dia))
            {
                var day = DayOfWeek.Sunday;

                switch (item.Key)
                {
                    case 6:
                        day = DayOfWeek.Friday;
                        break;

                    case 2:
                        day = DayOfWeek.Monday;
                        break;

                    case 7:
                        day = DayOfWeek.Saturday;
                        break;

                    case 1:
                        day = DayOfWeek.Sunday;
                        break;

                    case 5:
                        day = DayOfWeek.Thursday;
                        break;

                    case 3:
                        day = DayOfWeek.Tuesday;
                        break;

                    case 4:
                        day = DayOfWeek.Wednesday;
                        break;
                }

                var tiempos = item.ToList().Select(s => new ScheduleModel
                {
                    NumberDay = item.Key,
                    Id = s.id,
                    Inicio = TimeSpan.Parse(s.inicio),
                    Fin = TimeSpan.Parse(s.fin)
                });

                var model = new ScheduleDayModel
                {
                    Day = day.ToString(),
                    Estado = true,
                    NumberDay = item.Key,
                    Id = 0,
                    Tiempo = ""
                };

                var strtiempos = new List<string>(tiempos.Count());
                foreach (var tiempo in tiempos)
                {
                    strtiempos.Add(tiempo.Inicio.ToString() + " - " + tiempo.Fin.ToString());
                    model.Add(tiempo);
                }

                model.Tiempo = string.Join(",", strtiempos);

                list.Add(model);
            }

            Horarios = new ObservableCollection<ScheduleDayModel>(list);
        }

        private async void BtnEdit_Clicked(object obj)
        {
            MessagingCenter.Instance.Unsubscribe<Application>(this, "ReloadSchedule");
            MessagingCenter.Instance.Subscribe<Application>(this, "ReloadSchedule", async (app) => await LoadSchedule());
            await Navigation.PushAsync(new ScheduleEditPage());
        }
    }
}
