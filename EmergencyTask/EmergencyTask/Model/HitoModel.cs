using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class HitoModel : ModelBase
    {

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

        #region BindableProperty Amount
        /// <summary>
        /// Amount de la propiedad bindable
        /// </summary>
        private string amount;
        public string Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty State
        /// <summary>
        /// State de la propiedad bindable
        /// </summary>
        private string state;
        public string State
        {
            get { return state; }
            set { state = value; OnPropertyChanged(); }
        }

        public int IdState { get; internal set; }
        public int IdHito { get; internal set; }
        public double Price { get; internal set; }
        #endregion

        #region BindableProperty TapMenu
        /// <summary>
        /// TapMenu de la propiedad bindable
        /// </summary>
        private Command tapmenu;
        public Command TapMenu
        {
            get { return tapmenu; }
            set { tapmenu = value; OnPropertyChanged(); }
        }

        public string Charge { get; internal set; }
        public double Cantidad { get; internal set; }
        public int TrabajoTerminado { get; internal set; }
        public int Trabajador { get; internal set; }
        public int Cliente { get; internal set; }
        public string TicketImage { get; internal set; }
        public string TicketDetail { get; internal set; }
        public bool HasTicket { get; internal set; }
        public int TicketId { get; internal set; }
        #endregion

        #region Notified Property IsOptionsVisible
        /// <summary>
        /// IsOptionsVisible
        /// </summary>
        private bool isoptionsvisible;
        public bool IsOptionsVisible
        {
            get { return isoptionsvisible; }
            set { isoptionsvisible = value; OnPropertyChanged(); }
        }

        public int RequestServiceId { get; internal set; }
        #endregion

        public bool IsFromExtras { get; set; }
    }
}