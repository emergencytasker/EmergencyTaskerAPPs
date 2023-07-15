using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class StudyViewModel : ViewModelBase
    {

        #region Notified Property Grados
        /// <summary>
        /// Grados
        /// </summary>
        private IList<string> grados;
        public IList<string> Grados
        {
            get { return grados; }
            set { grados = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Titulo
        /// <summary>
        /// Titulo
        /// </summary>
        private string titulo;
        public string Titulo
        {
            get { return titulo; }
            set { titulo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Institucion
        /// <summary>
        /// Institucion
        /// </summary>
        private string institucion;
        public string Institucion
        {
            get { return institucion; }
            set { institucion = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Cedula
        /// <summary>
        /// Cedula
        /// </summary>
        private string cedula;
        public string Cedula
        {
            get { return cedula; }
            set { cedula = value; OnPropertyChanged(); }
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

        #region Notified Property Grado
        /// <summary>
        /// Grado
        /// </summary>
        private string grado;
        public string Grado
        {
            get { return grado; }
            set { grado = value; OnPropertyChanged(); }
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

        public Study Study { get; set; }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            BtnClose = new Command(BtnClose_Clicked);

            var me = Usuario.GetUserLogin();
            if (me == null) return;

            IsBusy = true;

            Grados = new List<string>(5)
            {
                "Middle School", "High School"
            };

            Save = new ExtendCommand(Save_Study, new UserValidator(), new InternetValidator());

            Study = (await Client.Study.Where(new Study
            {
                idusuario = me.id
            })).FirstOrDefault();

            if (Study == null)
            {
                IsBusy = false;
                return;
            }

            Grado = Study.grado;
            Titulo = Study.titulo;
            Institucion = Study.institucion;
            IsBusy = false;
        }

        private async void BtnClose_Clicked(object obj)
        {
            await Navigation.PopPopupAsync();
        }

        private async void Save_Study(object obj, IExecuteValidator[] args)
        {
            if (!args.TryGetComparator(out Usuario me)) return;

            IsBusy = true;

            if (!FormValid(out string message))
            {
                Toast(message);
                IsBusy = false;
                return;
            }

            if (Study == null)
            {
                // insert
                Study = await Client.Study.Add(new Study
                {
                    grado = Grado,
                    institucion = Institucion,
                    titulo = Titulo,
                    idusuario = me.id
                });
            }
            else
            {
                // update
                Study = await Client.Study.Update(Study.id, new Dictionary<string, string>
                {
                    { nameof(Study.grado), Grado }
                });
            }


            if (Study == null || Study.id <= 0)
            {
                Toast(AppResource.NoGuardoCambios);
            }
            else
            {
                Toast(AppResource.CambiosGuardados);
                MessagingCenter.Instance.Send(App, "Change", Study);
                await Navigation.PopPopupAsync();
            }

            IsBusy = false;
        }

        private bool FormValid(out string message)
        {
            message = "";

            if (string.IsNullOrEmpty(Grado))
            {
                message = AppResource.SeleccionarGrado;
                return false;
            }

            if (string.IsNullOrEmpty(Titulo))
            {
                message = AppResource.SeleccionarGrado;
                return false;
            }

            if (string.IsNullOrEmpty(Institucion))
            {
                message = AppResource.SeleccionarGrado;
                return false;
            }
            
            return true;
        }
    }
}
