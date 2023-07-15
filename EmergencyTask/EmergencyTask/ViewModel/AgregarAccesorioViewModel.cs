using EmergencyTask.Model;
using EmergencyTask.Strings;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class AgregarAccesorioViewModel : ViewModelBase
    {

        #region Notified Property Nombre
        /// <summary>
        /// Nombre
        /// </summary>
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged(); if (Model != null) { Model.Nombre = value; Agregar.ChangeCanExecute(); } }
        }
        #endregion

        #region Notified Property Cantidad
        /// <summary>
        /// Cantidad
        /// </summary>
        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; OnPropertyChanged(); if (Model != null) { Model.Cantidad = value; Agregar.ChangeCanExecute(); } }
        }
        #endregion

        #region Notified Property Agregar
        /// <summary>
        /// Agregar
        /// </summary>
        private Command agregar;
        public Command Agregar
        {
            get { return agregar; }
            set { agregar = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnClose
        /// <summary>
        /// BtnClose
        /// </summary>
        private Command btnclose;
        public Command BtnClose
        {
            get { return btnclose; }
            set { btnclose = value; OnPropertyChanged(); }
        }
        #endregion

        public AccesorioModel Model { get; set; }

        public AgregarAccesorioViewModel(Action<AccesorioModel> accesorio, AccesorioModel model = null)
        {
            if(model != null)
            {
                Nombre = model.Nombre;
                Cantidad = model.Cantidad;
            }
            Model = model ?? new AccesorioModel();
            Agregar = new Command(() =>
            {
                if(Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    accesorio?.Invoke(Model);
                }
                else
                {
                    Toast(AppResource.RequeresInternet);
                }
            }, () => 
            {
                var validation = Model.Validation();
                return validation;
            });

            BtnClose = new Command(BtnClose_Command);
        }

        private async void BtnClose_Command(object obj)
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch { }
        }
    }
}
