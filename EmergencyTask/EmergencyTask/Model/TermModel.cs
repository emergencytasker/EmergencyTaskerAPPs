using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class TermModel : ModelBase
    {

        #region BindableProperty TermPage
        /// <summary>
        /// TermPage de la propiedad bindable
        /// </summary>
        private ImageSource termpage;
        public ImageSource TermPage
        {
            get { return termpage; }
            set { termpage = value; OnPropertyChanged(); }
        }
        #endregion

    }
}
