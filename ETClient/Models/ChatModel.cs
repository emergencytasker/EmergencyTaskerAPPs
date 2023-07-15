namespace ETClient.Models
{
    public class ChatModel : ModelBase
    {
        public string ImageUser { set; get; }

        #region BindableProperty NameUser
        /// <summary>
        /// NameUser de la propiedad bindable
        /// </summary>
        private string nameuser;
        public string NameUser
        {
            get { return nameuser; }
            set { nameuser = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty LastUpdate
        /// <summary>
        /// LastUpdate de la propiedad bindable
        /// </summary>
        private string lastupdate;
        public string LastUpdate
        {
            get { return lastupdate; }
            set { lastupdate = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty Messenger
        /// <summary>
        /// Messenger de la propiedad bindable
        /// </summary>
        private string messenger;
        public string Messenger
        {
            get { return messenger; }
            set { messenger = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty IsPenddingVisible
        /// <summary>
        /// IsPenddingVisible de la propiedad bindable
        /// </summary>
        private bool ispenddingvisible;
        public bool IsPenddingVisible
        {
            get { return ispenddingvisible; }
            set { ispenddingvisible = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty PenddingMessage
        /// <summary>
        /// PenddingMessage de la propiedad bindable
        /// </summary>
        private int penddingmessage;
        public int PenddingMessage
        {
            get { return penddingmessage; }
            set { penddingmessage = value; OnPropertyChanged(); IsPenddingVisible = value > 0; }
        }

        public int Cliente { get; internal set; }
        public int Trabajador { get; internal set; }
        public string Date { get; internal set; }
        #endregion

    }
}
