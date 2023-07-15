namespace ETClient.Models
{
    public class NotificationModel : ModelBase
    {


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


        #region Notified Property SubTitle
        /// <summary>
        /// SubTitle
        /// </summary>
        private string subtitle;
        public string Subtitle
        {
            get { return subtitle; }
            set { subtitle = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property HoraFecha
        /// <summary>
        /// HoraFecha
        /// </summary>
        private string horafecha;
        public string HoraFecha
        {
            get { return horafecha; }
            set { horafecha = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Id
        /// <summary>
        /// Id
        /// </summary>
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Data
        /// <summary>
        /// Data
        /// </summary>
        private string data;
        public string Data
        {
            get { return data; }
            set { data = value; OnPropertyChanged(); }
        }

        public int IdAction { get; internal set; }
        #endregion

    }
}
