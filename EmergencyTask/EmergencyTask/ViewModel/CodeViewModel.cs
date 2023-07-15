using System;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class CodeViewModel : ViewModelBase
    {

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

        #region BindableProperty Codigo
        /// <summary>
        /// Codigo de la propiedad bindable
        /// </summary>
        private string codigo;
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; OnPropertyChanged(); }
        }
        #endregion

        private string CodeToMatch { get; }
        public int IdUsuario { get; }

        public CodeViewModel(string code, int idusuario)
        {
            CodeToMatch = code;
            IdUsuario = idusuario;
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            BtnAceptar = new ExtendCommand(BtnAceptar_Clicked, () => !string.IsNullOrEmpty(Codigo));
        }

        private async void BtnAceptar_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if(Codigo == CodeToMatch)
            {
                await Navigation.PushAsync(new NewPasswordPage
                {
                    BindingContext = new NewPasswordViewModel(IdUsuario)
                });
            }
            else
            {
                Toast(AppResource.CodigosNoCoinciden);
            }
        }
    }
}
