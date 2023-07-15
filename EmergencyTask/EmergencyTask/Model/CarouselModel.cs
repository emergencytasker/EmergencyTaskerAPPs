using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class CarouselModel : ModelBase
    {

        #region BindableProperty Wallpaper
        /// <summary>
        /// Wallpaper de la propiedad bindable
        /// </summary>
        private ImageSource wallpaper;
        public ImageSource Wallpaper
        {
            get { return wallpaper; }
            set { wallpaper = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Icon
        /// <summary>
        /// Icon de la propiedad bindable
        /// </summary>
        private ImageSource icon;
        public ImageSource Icon
        {
            get { return icon; }
            set { icon = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Text
        /// <summary>
        /// Text de la propiedad bindable
        /// </summary>
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Animation
        /// <summary>
        /// Animation de la propiedad bindable
        /// </summary>
        private ImageSource animation;
        public ImageSource Animation
        {
            get { return animation; }
            set { animation = value; OnPropertyChanged(); }
        }
        #endregion


    }
}
