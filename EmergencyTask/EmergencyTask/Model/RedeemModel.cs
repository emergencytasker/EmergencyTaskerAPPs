using EmergencyTask.ViewModel.Commands;

namespace EmergencyTask.Model
{
    public class RedeemModel : ModelBase
    {

        #region BindableProperty Title
        /// <summary>
        /// Title de la propiedad bindable
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Redeem
        /// <summary>
        /// Redeem de la propiedad bindable
        /// </summary>
        private string redeem;
        public string Redeem
        {
            get { return redeem; }
            set { redeem = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Now
        /// <summary>
        /// Now de la propiedad bindable
        /// </summary>
        private double now;
        public double Now
        {
            get { return now; }
            set { now = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Goal
        /// <summary>
        /// Goal de la propiedad bindable
        /// </summary>
        private double goal;
        public double Goal
        {
            get { return goal; }
            set { goal = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Progress
        /// <summary>
        /// Progress de la propiedad bindable
        /// </summary>
        public double Progress
        {
            get { return Now/Goal; }
        }
        #endregion

        #region Notified Property IsRedeedRewardVisible
        /// <summary>
        /// IsRedeedRewardVisible
        /// </summary>
        private bool isredeemrewardvisible;
        public bool IsRedeedRewardVisible
        {
            get { return isredeemrewardvisible; }
            set { isredeemrewardvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property RedeemReward
        /// <summary>
        /// RedeemReward
        /// </summary>
        private ExtendCommand redeemreward;
        public ExtendCommand RedeemReward
        {
            get { return redeemreward; }
            set { redeemreward = value; OnPropertyChanged(); }
        }
        #endregion

        public int Id { get; set; }
        public int IdReward { get; set; }
        public string Detail { get; set; }
    }
}
