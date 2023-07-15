using EmergencyTask.API;
using EmergencyTask.Strings;
using System;
using System.Net.Mail;
using System.Threading;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class RegisterViewModel : ViewModelBase
    {

        #region BindableProperty Email
        /// <summary>
        /// Email de la propiedad bindable
        /// </summary>
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Password
        /// <summary>
        /// Password de la propiedad bindable
        /// </summary>
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty NameUser
        /// <summary>
        /// NameUser de la propiedad bindable
        /// </summary>
        private string nameuser;
        public string NameUser
        {
            get { return nameuser; }
            set { nameuser = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty ConfirmPassword
        /// <summary>
        /// ConfirmPassword de la propiedad bindable
        /// </summary>
        private string confirmpassword;
        public string ConfirmPassword
        {
            get { return confirmpassword; }
            set { confirmpassword = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapTerms
        /// <summary>
        /// TapTerms de la propiedad bindable
        /// </summary>
        private Command tapterms;
        public Command TapTerms
        {
            get { return tapterms; }
            set { tapterms = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty SignIn
        /// <summary>
        /// SignIn de la propiedad bindable
        /// </summary>
        private Command signin;
        public Command SignIn
        {
            get { return signin; }
            set { signin = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty SignUp
        /// <summary>
        /// SignUp de la propiedad bindable
        /// </summary>
        private Command signup;
        public Command SignUp
        {
            get { return signup; }
            set { signup = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsTermsAndConditionsChecked
        /// <summary>
        /// IsTermsAndConditionsChecked
        /// </summary>
        private bool istermsandconditionschecked;
        public bool IsTermsAndConditionsChecked
        {
            get { return istermsandconditionschecked; }
            set { istermsandconditionschecked = value; OnPropertyChanged(); }
        }
        #endregion

        public RegisterViewModel()
        {
            SignIn = new Command(SignIn_Clicked);
            SignUp = new Command(SignUp_Clicked);
            TapTerms = new Command(Terms_Clicked);
        }

        private async void Terms_Clicked(object obj)
        {
            await Navigation.PushAsync(new PDFRenderPage());
        }

        private async void SignUp_Clicked(object obj)
        {
            IsBusy = true;

            

            if (string.IsNullOrEmpty(NameUser))
            {
                Toast(AppResource.IngresaNombre);
                IsBusy = false;
                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                Toast(AppResource.IngresaEmail);
                IsBusy = false;
                return;
            }

            try
            {
                new MailAddress(Email);
            }
            catch
            {
                Toast(AppResource.IngresaUnEmailValido);
                IsBusy = false;
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                Toast(AppResource.IngresaPassword);
                IsBusy = false;
                return;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                Toast(AppResource.ConfirmarContraseña);
                IsBusy = false;
                return;
            }

            if (Password != ConfirmPassword)
            {
                Toast(AppResource.ContraseñaNoCoincide);
                IsBusy = false;
                return;
            }

            if (!IsTermsAndConditionsChecked)
            {
                Toast(AppResource.DebesAceptarTerminos);
                IsBusy = false;
                return;
            }

            Email = Email.Trim().TrimStart().TrimEnd();
            Password = Password.Trim().TrimStart().TrimEnd();
            ConfirmPassword = ConfirmPassword.Trim().TrimStart().TrimEnd();

            var user = await Client.User.Add(new API.ER.User
            {
                email = Email,
                nombre = NameUser,
                password = Password,
                idperfil = (int) App.Perfil,
                lenguaje = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName
            });

            if(user != null && user.email == Email && user.id > 0)
            {
                if (App.Perfil == API.Enum.Perfil.Client)
                {
                    await Client.VerificationMail(user.id, user.email);
                    Toast(AppResource.ActivarCuenta);
                }
                else
                {
                    await Client.VerificationMail(user.id, user.email);
                    Toast(AppResource.ActivarCuentaTasker);
                }
                await Navigation.PopAsync();
            }
            else
            {
                Toast(AppResource.CorreoYaRegistrado);
            }

            IsBusy = false;
        }

        private async void SignIn_Clicked(object obj)
        {
            await Navigation.PopAsync();
        }
    }
}
