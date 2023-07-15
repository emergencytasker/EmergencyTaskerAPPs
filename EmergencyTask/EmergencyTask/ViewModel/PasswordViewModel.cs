using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using System;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class PasswordViewModel : ViewModelBase
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

        #region BindableProperty BtnAceptar
        /// <summary>
        /// BtnAceptar de la propiedad bindable
        /// </summary>
        private ExtendCommand btnaceptar;
        public ExtendCommand BtnAceptar
        {
            get { return btnaceptar; }
            set { btnaceptar = value; OnPropertyChanged(); }
        }
        #endregion

        public PasswordViewModel()
        {
            var emailvalidator = new ExecuteValidator(AppResource.EmailValido, () =>
            {
                return !string.IsNullOrEmpty(Email);
            });
            BtnAceptar = new ExtendCommand(BtnAceptar_Clicked, new InternetValidator(), emailvalidator);
        }

        private async void BtnAceptar_Clicked(object obj, IExecuteValidator[] validators)
        {
            IsBusy = true;
            if (string.IsNullOrEmpty(Email))
            {
                Toast(AppResource.IngresaCorreo);
                IsBusy = false;
                return;
            }

            var user = (await Client.User.Where(new User
            {
                email = Email,
                idperfil = (int)App.Perfil
            })).FirstOrDefault();

            if (user == null)
            {
                Toast(AppResource.RevisaEmail);
                IsBusy = false;
                return;
            }

            var code = (new Random().Next(10000, 99999)).ToString();
            var send = await Client.SendMail(Email, AppResource.CodigoRecuperacion, string.Format(AppResource.EsteCodigo, user.nombre, code));
            if (!send)
            {
                Toast(AppResource.SinContinuar);
                IsBusy = false;
                return;
            }
            
            await Navigation.PushAsync(new CodePage
            {
                BindingContext = new CodeViewModel(code, user.id)
            });
            IsBusy = false;
        }
    }
}
