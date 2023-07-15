using Xamarin.Forms;

namespace EmergencyTask.Model
{

    public enum TypeSocialNetwork
    {
        Facebook, Whatsapp, Instagram, Twitter
    }

    public class SocialNetworkModel : ModelBase
    {
        public TypeSocialNetwork Id { get; set; }

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

        #region Notified Property Title
        /// <summary>
        /// Title
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }
        #endregion
    }
}
