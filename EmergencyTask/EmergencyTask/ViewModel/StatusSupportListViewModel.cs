using EmergencyTask.Model;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class StatusSupportListViewModel : ViewModelBase
    {
        public SupportModel Support { get; set; }


        #region Notified Property State
        /// <summary>
        /// State
        /// </summary>
        private StateModel state;
        public StateModel State
        {
            get { return state; }
            set { state = value; OnPropertyChanged(); }
        }
        #endregion


        #region Notified Property Status
        /// <summary>
        /// Status
        /// </summary>
        private ObservableCollection<StateModel> status;
        public ObservableCollection<StateModel> Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }
        #endregion

        public StatusSupportListViewModel(SupportModel support)
        {
            Support = support;
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            Status = new ObservableCollection<StateModel>
            {
                new StateModel
                {
                    Id = 10,
                    Status = "Pending",
                    Comentary = "",
                    Date = DateTime.Now,
                }
            };
        }
    }
}
