using EmergencyTask.Model;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class TicketPopupViewModel : ViewModelBase
    {

        #region Notified Property Imagen
        /// <summary>
        /// Image
        /// </summary>
        private string image;
        public string Imagen
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Detalle
        /// <summary>
        /// Detalle
        /// </summary>
        private string detalle;
        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; OnPropertyChanged(); }
        }
        #endregion

        public int Id { get; private set; }

        public TicketPopupViewModel(HitoModel model)
        {
            Imagen = model.TicketImage;
            Detalle = model.TicketDetail;
            Id = model.TicketId;
        }

        public override void OnAppearing(Page page)
        {
            
        }

    }
}