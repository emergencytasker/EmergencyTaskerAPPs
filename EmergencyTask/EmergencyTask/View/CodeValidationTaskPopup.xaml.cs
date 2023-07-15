using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodeValidationTaskPopup : PopupPage
    {

        #region Notified Property Method
        /// <summary>
        /// Method
        /// </summary>
        private string method;
        public string Method
        {
            get { return method; }
            set { method = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Validation
        /// <summary>
        /// Validation
        /// </summary>
        private Command validation;
        public Command Validation
        {
            get { return validation; }
            set { validation = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Code
        /// <summary>
        /// Code
        /// </summary>
        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ResendCode
        /// <summary>
        /// ResendCode
        /// </summary>
        private Command resendcode;
        public Command ResendCode
        {
            get { return resendcode; }
            set { resendcode = value; OnPropertyChanged(); }
        }
        #endregion

        public Action OK { get; set; }
        public string CurrentCode { get; set; }

        public CodeValidationTaskPopup()
        {
            InitializeComponent();
            BindingContext = this;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            ResendCode = new Command(ResendCode_Command);
            Validation = new Command(Validation_Clicked);
            var send = await SendSode();
            if (!send) await DisplayAlert(AppResource.Info, AppResource.ErrorAlEnviarElCodigo, AppResource.Aceptar);
            IsBusy = false;
        }

        private async Task<bool> SendSode()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null)
            {
                await Navigation.PopPopupAsync();
                return false;
            }

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert(AppResource.Info, AppResource.RequeresInternet, AppResource.Aceptar);
                return false;
            }

            CurrentCode = new Random().Next(10000, 99999).ToString();
            var message = string.Format(AppResource.CodeMessage, usuario.nombre, CurrentCode);
            var sms = false;
            if (usuario.telefonoverificado == 1)
                sms = Client.SendSms(usuario.telefono, message, "EmergencyTask");
            var email = await Client.SendMail(usuario.email, AppResource.CodigoDeVerificacion, message);
            Method = AppResource.CodigoEnviado;
            Debug.WriteLine($"[SendSode] {message}");
            return sms || email;
        }

        private async void ResendCode_Command(object obj)
        {
            if (await SendSode()) return;
            await DisplayAlert(AppResource.Info, AppResource.ErrorAlEnviarElCodigo, AppResource.Aceptar);
        }

        private async void Validation_Clicked(object obj)
        {
            if (string.IsNullOrEmpty(Code))
            {
                await DisplayAlert(AppResource.Info, AppResource.CodigoNoValido, AppResource.Aceptar);
                return;
            }

            if(Code != CurrentCode)
            {
                await DisplayAlert(AppResource.Info, AppResource.CodigoNoCoincideConElIngresado, AppResource.Aceptar);
                return;
            }

            OK?.Invoke();

            await Navigation.PopPopupAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
    }
}