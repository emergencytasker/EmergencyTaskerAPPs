using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class EvidenceModel : ModelBase
    {

        public int Id { get; set; }

        #region Notified Property Image
        /// <summary>
        /// Image
        /// </summary>
        private ImageSource image;
        public ImageSource Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Options
        /// <summary>
        /// TapDelete
        /// </summary>
        private ICommand options;
        public ICommand Options
        {
            get { return options; }
            set { options = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Descripcion
        /// <summary>
        /// Descripcion de la propiedad bindable
        /// </summary>
        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TieneDescripcion
        /// <summary>
        /// TieneDescripcion de la propiedad bindable
        /// </summary>
        private bool tienedescripcion;
        public bool TieneDescripcion
        {
            get { return tienedescripcion; }
            set { tienedescripcion = value; OnPropertyChanged(); }
        }
        #endregion
        
        #region Notified Property TapDelete
        /// <summary>
        /// TapDelete
        /// </summary>
        private ICommand tapdelete;
        public ICommand TapDelete
        {
            get { return tapdelete; }
            set { tapdelete = value; OnPropertyChanged(); }
        }
        #endregion
    }
}
