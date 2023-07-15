using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class CanceledReasonViewModel : ViewModelBase
    {
        #region Notified Property BtnClose
        /// <summary>
        /// BtnClose
        /// </summary>
        private ICommand btnclose;
        private int IdSolicitudServicio;

        public CanceledReasonViewModel(int idsolicitudservicio)
        {
            IdSolicitudServicio = idsolicitudservicio;
        }

        public ICommand BtnClose
        {
            get { return btnclose; }
            set { btnclose = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Reasons
        /// <summary>
        /// Reasons
        /// </summary>
        private List<Reason> reasons;
        public List<Reason> Reasons
        {
            get { return reasons; }
            set { reasons = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Reason
        /// <summary>
        /// Reason
        /// </summary>
        private Reason reason;
        public Reason Reason
        {
            get { return reason; }
            set { reason = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Description
        /// <summary>
        /// Description
        /// </summary>
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnSend
        /// <summary>
        /// BtnSend
        /// </summary>
        private ICommand btnsend;
        public ICommand BtnSend
        {
            get { return btnsend; }
            set { btnsend = value; OnPropertyChanged(); }
        }
        #endregion

        public System.Action Completed { get; internal set; }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;
            Reasons = await GetReasons();
            BtnClose = new Command(BtnClose_Clicked);
            BtnSend = new Command(BtnSend_Clicked);
            IsBusy = false;
        }

        /// <summary>
        /// Ejecuta el boton enviar
        /// </summary>
        /// <param name="obj"></param>
        private async void BtnSend_Clicked(object obj)
        {
            if(Reason == null)
            {
                Toast(Strings.AppResource.CanceledReasonViewModel_BtnSend_Clicked_EligeUnaRazón);
                return;
            }

            if (IsBusy) return;
            IsBusy = true;

            var canceledservice = await Client.Cenacelservice.Add(new Cancelservice
            {
                comentario = Description,
                idsolicitudservicio = IdSolicitudServicio,
                idrazon = Reason.idrazon
            });

            if(canceledservice == null)
            {
                Toast(Strings.AppResource.CanceledReasonViewModel_BtnSend_Clicked_NoPodemosGuardarTuRazónDeCancelaciónIntentaDeNuevo);
                IsBusy = false;
                return;
            }

            Completed?.Invoke();
            try { await PopupNavigation.Instance.PopAsync(); } catch { }

            IsBusy = false;
        }

        /// <summary>
        /// Obtiene la lista de razones
        /// </summary>
        /// <returns></returns>
        public async Task<List<Reason>> GetReasons()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return new List<Reason>();
            var lenguaje = usuario.lenguaje;
            if (string.IsNullOrEmpty(lenguaje))
                lenguaje = "en";
            var lang = (await Client.Language.Where(new Language
            {
                codigo = lenguaje
            })).FirstOrDefault(f => f.codigo == lenguaje);
            int idlenguaje = 1;
            if (lang != null)
                idlenguaje = lang.id;
            var reasons = await Client.Reasons.Where(new Reason
            {
                idlenguaje = idlenguaje
            }) ?? new List<Reason>(0);
            return reasons.OrderBy(r => r.idrazon).ToList();
        }

        private async void BtnClose_Clicked(object obj)
        {
            try
            {
                await PopupNavigation.Instance.PopAllAsync();
            }
            catch { }
        }
    }
}