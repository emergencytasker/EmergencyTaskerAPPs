using EmergencyTask.Model;
using LightForms.Commands;
using LightForms.Validations;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class AddSupportViewModel : ViewModelBase
    {

        #region Notified Property Categories
        /// <summary>
        /// Categories
        /// </summary>
        private CategoriesModel categories;
        public CategoriesModel Categories
        {
            get { return categories; }
            set { categories = value; OnPropertyChanged(); }
        }
        #endregion


        #region Notified Property Services
        /// <summary>
        /// Services
        /// </summary>
        private ServiceModel services;
        public ServiceModel Services
        {
            get { return services; }
            set { services = value; OnPropertyChanged(); }
        }
        #endregion


        #region Validatable Property Description
        private ValidatableObject<string> description = new ValidatableObject<string>
        {
            IsValid = false,
            Validations = new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>
                {
                    ValidationMessage = "Invalid Description"
                }
            }
        };
        public ValidatableObject<string> Description
        {
            get => description;
            set { description = value; OnPropertyChanged(); }
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

        #region Notified Property BtnAccept
        /// <summary>
        /// BtnAccept
        /// </summary>
        private ICommand btnaccept;
        public ICommand BtnAccept
        {
            get { return btnaccept; }
            set { btnaccept = value; OnPropertyChanged(); }
        }
        #endregion

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            BtnAccept = new LightForms.Commands.Command(BtnAccept_Clicked);
            BtnClose = new LightForms.Commands.Command(BtnClose_Clicked);
        }

        private async void BtnClose_Clicked(object obj)
        {
            if(PopupNavigation.Instance.PopupStack.Count>0)
                await PopupNavigation.Instance.PopAsync();
        }

        private void BtnAccept_Clicked(object obj)
        {

        }
    }
}
