using System;
using System.Collections.Generic;
using System.Windows.Input;
using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class WorkViewModel : ViewModelBase
    {

        #region Notified Property Work
        /// <summary>
        /// Work
        /// </summary>
        private WorkModel work;
        public WorkModel Work
        {
            get { return work; }
            set { work = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Save
        /// <summary>
        /// Save
        /// </summary>
        private ExtendCommand save;
        public ExtendCommand Save
        {
            get { return save; }
            set { save = value; OnPropertyChanged(); }
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

        public WorkViewModel()
        {
            
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            if(Work == null) Work = new WorkModel();
            Work.Inicio = DateTime.Now;
            Work.Fin = DateTime.Now;
            BtnClose = new Command(BtnClose_Clicked);

            Save = new ExtendCommand(Save_Command, new UserValidator(), new InternetValidator());
        }

        private async void BtnClose_Clicked(object obj)
        {
            try
            {
                await Navigation.PopPopupAsync();
            }
            catch { }
        }

        private async void Save_Command(object arg1, IExecuteValidator[] arg2)
        {
            if (IsBusy) return;
            IsBusy = true;

            if (!arg2.TryGetComparator(out Usuario me))
            {
                IsBusy = false;
                return;
            }

            if (!FormValid(out string message))
            {
                IsBusy = false;
                DisplayAlert(message, AppResource.Aceptar);
                return;
            }

            Work work;

            if(Work.Id > 0)
            {
                // update
                work = await Client.Work.Update(Work.Id, new Dictionary<string, string>
                {
                    { nameof(API.ER.Work.descripcion), Work.Descripcion },
                    { nameof(API.ER.Work.empresa), Work.Empresa },
                    { nameof(API.ER.Work.fin), Work.FechaFin },
                    { nameof(API.ER.Work.inicio), Work.FechaInicio },
                    { nameof(API.ER.Work.puesto), Work.Puesto }
                });
            }
            else
            {
                // insert
                work = await Client.Work.Add(new Work
                {
                    puesto = Work.Puesto,
                    inicio = Work.FechaInicio,
                    descripcion = Work.Descripcion,
                    empresa = Work.Empresa,
                    fin = Work.FechaFin,
                    idusuario = me.id
                });
            }

            if(work == null || work.id <= 0)
            {
                Toast(AppResource.NoGuardoTrabajo);
            }
            else
            {
                Toast(AppResource.TrabajoGuardado);
                MessagingCenter.Instance.Send(App, "Change", work);
                await Navigation.PopPopupAsync();
            }

            IsBusy = false;
        }

        private bool FormValid(out string message)
        {
            message = "";

            if(string.IsNullOrEmpty(Work.Empresa))
            {
                message = AppResource.IngresaEmpresa;
                return false;
            }

            if (string.IsNullOrEmpty(Work.Puesto))
            {
                message = AppResource.IngresUnPuesto;
                return false;
            }

            if (string.IsNullOrEmpty(Work.FechaInicio))
            {
                message = AppResource.SeleccionaUnaFechaDeInicio;
                return false;
            }

            if (string.IsNullOrEmpty(Work.FechaFin))
            {
                message = AppResource.SeleccionaUnaFechaDeFin;
                return false;
            }

            if (string.IsNullOrEmpty(Work.Descripcion))
            {
                message = AppResource.IngresaUnaDescripcionDeTrabajo;
                return false;
            }

            return true;
        }
    }
}