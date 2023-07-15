using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class TicketListModel : ModelBase
    {

        #region BindableProperty Image
        /// <summary>
        /// Image de la propiedad bindable
        /// </summary>
        private ImageSource image;
        public ImageSource Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Description
        /// <summary>
        /// Description de la propiedad bindable
        /// </summary>
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
        #endregion

        public int Id { get; set; }

    }
}
