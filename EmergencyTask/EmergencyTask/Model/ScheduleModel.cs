using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EmergencyTask.Model
{

    public class ScheduleDayModel : ModelBase
    {

        #region BindableProperty Estado
        /// <summary>
        /// Estado de la propiedad bindable
        /// </summary>
        private bool estado;
        public bool Estado
        {
            get { return estado; }
            set { estado = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Day
        /// <summary>
        /// Day de la propiedad bindable
        /// </summary>
        private string day;
        public string Day
        {
            get { return day; }
            set { day = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Schedules
        /// <summary>
        /// Schedules
        /// </summary>
        private ObservableCollection<ScheduleModel> schedules;
        public ObservableCollection<ScheduleModel> Schedules
        {
            get { return schedules; }
            set { schedules = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ListHeight
        /// <summary>
        /// ListHeight
        /// </summary>
        private double listheight;
        public double ListHeight
        {
            get { return listheight; }
            set { listheight = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Inicio
        /// <summary>
        /// Inicio
        /// </summary>
        private string tiempo;
        public string Tiempo
        {
            get { return tiempo; }
            set { tiempo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Add
        /// <summary>
        /// Add
        /// </summary>
        private ICommand addschedule;
        public ICommand AddSchedule
        {
            get { return addschedule; }
            set { addschedule = value; OnPropertyChanged(); }
        }
        #endregion

        public int Id { get; internal set; }
        public int NumberDay { get; internal set; }

        public ScheduleDayModel()
        {

        }

        public void Remove(ScheduleModel schedulemodel)
        {
            if (Schedules == null) return;
            Schedules.Remove(schedulemodel);
            RefreshUI();
        }

        private void RefreshUI()
        {
            ListHeight = Schedules.Count * 40;
            Estado = Schedules.Count > 0;
        }

        public void Add(ScheduleModel scheduleModel)
        {
            if (Schedules == null) Schedules = new ObservableCollection<ScheduleModel>();
            Schedules.Add(scheduleModel);
            RefreshUI();
        }

        public bool IsValid()
        {
            foreach (var schedule in Schedules)
            {
                if (schedule.Inicio >= schedule.Fin) return false;
                if (schedule.Inicio.TotalHours + 2 > schedule.Fin.TotalHours) return false;
            }

            if(Schedules.Count == 2)
            {
                var start = Schedules[0];
                var end = Schedules[1];
                return !(start.Inicio < end.Fin && end.Inicio < start.Fin);
            }

            return true;
        }
    }

    public class ScheduleModel : ModelBase
    {
        #region Notified Property Inicio
        /// <summary>
        /// Inicio
        /// </summary>
        private TimeSpan inicio;
        public TimeSpan Inicio
        {
            get { return inicio; }
            set { inicio = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Fin
        /// <summary>
        /// Fin
        /// </summary>
        private TimeSpan fin;
        public TimeSpan Fin
        {
            get { return fin; }
            set { fin = value; OnPropertyChanged(); }
        }

        public int Id { get; internal set; }
        public int NumberDay { get; internal set; }
        #endregion

        #region Notified Property Delete
        /// <summary>
        /// Delete
        /// </summary>
        private ICommand delete;
        public ICommand Delete
        {
            get { return delete; }
            set { delete = value; OnPropertyChanged(); }
        }
        #endregion
    }
}
