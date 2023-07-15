using EmergencyTask.Model;
using EmergencyTask.View;
using LightForms.Commands;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Command = LightForms.Commands.Command;

namespace EmergencyTask.ViewModel
{
    public class SupportListViewModel : ViewModelBase
    {

        #region Notified Property Support
        /// <summary>
        /// Support
        /// </summary>
        private SupportModel support;
        public SupportModel Support
        {
            get { return support; }
            set { support = value; OnPropertyChanged(); }
        }
        #endregion


        #region Notified Property Supports
        /// <summary>
        /// Supports
        /// </summary>
        private ObservableCollection<SupportModel> supports;
        public ObservableCollection<SupportModel> Supports
        {
            get { return supports; }
            set { supports = value; OnPropertyChanged(); }
        }
        #endregion


        #region Notified Property AddSupport
        /// <summary>
        /// AddSupport
        /// </summary>
        private ICommand addsupport;
        public ICommand AddSupport
        {
            get { return addsupport; }
            set { addsupport = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property SupportCommand
        /// <summary>
        /// SupportCommand
        /// </summary>
        private ICommand supportcommand;
        public ICommand SupportCommand
        {
            get { return supportcommand; }
            set { supportcommand = value; OnPropertyChanged(); }
        }
        #endregion

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            AddSupport = new Command(AddSupport_Clicked);
            SupportCommand = new Command(SupportCommand_Clicked);

            Supports = new ObservableCollection<SupportModel>
            {
                new SupportModel
                {
                    Id = 1,
                    Category = "",
                    DetailService = "",
                    IdStatus = 10,
                    Status = "",
                    Description = "",
                }
            };
        }

        private void SupportCommand_Clicked(object obj)
        {
            if (Support == null) return;
            Navigation.PushAsync(new StatusSupportListPage
            {
                BindingContext = new StatusSupportListViewModel(Support)
            });
        }

        private async void AddSupport_Clicked(object obj)
        {
            await PopupNavigation.Instance.PushAsync(new AddSupportPopup());
        }
    }
}
