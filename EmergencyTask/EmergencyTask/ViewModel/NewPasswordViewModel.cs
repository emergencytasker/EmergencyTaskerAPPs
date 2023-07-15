using EmergencyTask.API;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;

namespace EmergencyTask.ViewModel
{
    public class NewPasswordViewModel : ViewModelBase, IExtendCommandEvents
    {

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

        public int IdUsuario { get; set; }

        public NewPasswordViewModel(int idusuario)
        {
            IdUsuario = idusuario;
            BtnAceptar = new ExtendCommand(BtnAceptar_Clicked, () =>
            {
                return !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword);
            });
        }

        private async void BtnAceptar_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            IsBusy = true;
            var status = await Client.UpdatePassword(IdUsuario, Password);
            if (!status)
            {
                Toast(AppResource.PasswordNoActualizado);
                IsBusy = false;
                return;
            }
            Toast(AppResource.DatosActualizados);
            App.Restart();
            IsBusy = false;
        }
    }
}
