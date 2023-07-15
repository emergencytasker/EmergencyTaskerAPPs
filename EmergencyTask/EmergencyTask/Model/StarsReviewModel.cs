using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Simsa.Views.Rating
{
    public class StarsReviewModel : INotifyPropertyChanged
    {

        #region Notified Property Value
        /// <summary>
        /// Value
        /// </summary>
        private double val;
        public double Value
        {
            get { return val; }
            set { val = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property StarHeight
        /// <summary>
        /// StarHeight
        /// </summary>
        private double starheight;
        public double StarHeight
        {
            get { return starheight; }
            set { starheight = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property StarWidth
        /// <summary>
        /// StarWidth
        /// </summary>
        private double starwidth;
        public double StarWidth
        {
            get { return starwidth; }
            set { starwidth = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CanChange
        /// <summary>
        /// CanChange
        /// </summary>
        private bool canchange;
        public bool CanChange
        {
            get { return canchange; }
            set { canchange = value; OnPropertyChanged(); }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}