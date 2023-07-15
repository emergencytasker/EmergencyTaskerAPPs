using System;

namespace EmergencyTask.Model
{
    public class HamburguerMenuItem : ModelBase
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

        #region BindableProperty Image
        /// <summary>
        /// Image de la propiedad bindable
        /// </summary>
        private string image;
        public string Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
        #endregion

        public Type Page { get; set; }
        public Type ViewModel { get; set; }
        public object[] PageArgs { get; set; }
        public object[] ViewModelArgs { get; set; }

        #region BindableProperty Badge
        /// <summary>
        /// Badge de la propiedad bindable
        /// </summary>
        private int badge;
        public int Badge
        {
            get { return badge; }
            set { badge = value; OnPropertyChanged(); }
        }
        #endregion

        public HamburguerMenuItem()
        {
            PageArgs = new object[] { };
            ViewModelArgs = new object[] { };
        }
    }
}
