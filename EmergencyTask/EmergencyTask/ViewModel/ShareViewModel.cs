using EmergencyTask.Model;
using EmergencyTask.Services;
using EmergencyTask.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ShareViewModel : ViewModelBase
    {

        #region Notified Property SocialNetwork
        /// <summary>
        /// SocialNetwork
        /// </summary>
        private SocialNetworkModel socialnetwork;
        public SocialNetworkModel SocialNetwork
        {
            get { return socialnetwork; }
            set { socialnetwork = value; OnPropertyChanged(); }
        }
        #endregion


        #region Notified Property SocialNetworks
        /// <summary>
        /// SocialNetworks
        /// </summary>
        private ObservableCollection<SocialNetworkModel> socialnetworks;
        public ObservableCollection<SocialNetworkModel> SocialNetworks
        {
            get { return socialnetworks; }
            set { socialnetworks = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property SelectedSocialNetwork
        /// <summary>
        /// SelectedSocialNetwork
        /// </summary>
        private ICommand selectedsocialnetwork;
        public ICommand SelectedSocialNetwork
        {
            get { return selectedsocialnetwork; }
            set { selectedsocialnetwork = value; OnPropertyChanged(); }
        }
        #endregion

        public string Url { get; set; }

        #region Notified Property BtnCancel
        /// <summary>
        /// BtnCancel
        /// </summary>
        private ICommand btncancel;
        public ICommand BtnCancel
        {
            get { return btncancel; }
            set { btncancel = value; OnPropertyChanged(); }
        }
        #endregion

        public ShareViewModel(string url, IEnumerable<SocialNetworkModel> socialnetworks)
        {
            Url = url;
            SocialNetworks = new ObservableCollection<SocialNetworkModel>(socialnetworks);
        }

        public override void OnAppearing(Page page)
        {
            base.OnDisappearing(page);

            BtnCancel = new Command(BtnCancel_Clicked);
            SelectedSocialNetwork = new Command<SocialNetworkModel>(SelectedSocialNetwork_Clicked);
        }

        private void BtnCancel_Clicked(object obj)
        {
            throw new NotImplementedException();
        }

        private void SelectedSocialNetwork_Clicked(SocialNetworkModel socialnetwork)
        {
            if (socialnetwork == null) return;
            // var factory = new EnumFactory<TypeSocialNetwork, IShare>();
        }
    }
}
