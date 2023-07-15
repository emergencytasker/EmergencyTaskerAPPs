using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class TimeWorkListViewModel : ViewModelBase
    {

        #region BindableProperty WorkTime
        /// <summary>
        /// WorkTime de la propiedad bindable
        /// </summary>
        private WorkTimeModel workTime;
        public WorkTimeModel WorkTime
        {
            get { return workTime; }
            set { workTime = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty WorkTimes
        /// <summary>
        /// WorkTimes de la propiedad bindable
        /// </summary>
        private ObservableCollection<WorkTimeModel> worktimes;
        public ObservableCollection<WorkTimeModel> WorkTimes
        {
            get { return worktimes; }
            set { worktimes = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsLoaded
        /// <summary>
        /// IsLoaded
        /// </summary>
        private bool isloaded;
        public bool IsLoaded
        {
            get { return isloaded; }
            set { isloaded = value; OnPropertyChanged(); }
        }
        #endregion

        public int Idsolicitudservicio { get; set; }

        public TimeWorkListViewModel(int idsolicitudservicio)
        {
            Idsolicitudservicio = idsolicitudservicio;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;
            var hitos = (await Client.Hito.Where(new Hito
            {
                idsolicitudservicio = Idsolicitudservicio,
                estado = (int) HitoStatus.ReleaseFunds
            }) ?? new List<Hito>());
            var times = (await Client.Time.Where(new Time
            {
                idsolicitudservicio = Idsolicitudservicio
            }) ?? new List<Time>());
            WorkTimes = new ObservableCollection<WorkTimeModel>();
            if (hitos.Count() == 0 || times.Count() == 0)
            {
                IsBusy = false;
                return;
            }
            foreach (var hito in hitos)
            {
                var time = times.FirstOrDefault(t => t.chargeid == hito.chargeid);
                TimeSpan elapsedtime = TimeSpan.FromSeconds(0);
                if (time != null)
                    TimeSpan.TryParse(time.tiempo, out elapsedtime);
                WorkTimes.Add(new WorkTimeModel
                {
                    Tiempo = elapsedtime,
                    Total = App.Perfil == Perfil.Client ? hito.costofinal : (hito.cantidadtrabajador ?? hito.costofinal / 2)
                });
            }
            IsLoaded = true;
            IsBusy = false;
        }
    }
}
