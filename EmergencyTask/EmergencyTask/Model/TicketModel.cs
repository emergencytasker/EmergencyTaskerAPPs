using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class TicketModel : ModelBase
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

        #region BindableProperty Ticket
        /// <summary>
        /// Ticket de la propiedad bindable
        /// </summary>
        private ImageSource ticket;
        public ImageSource Ticket
        {
            get { return ticket; }
            set { ticket = value; OnPropertyChanged(); }
        }
        #endregion


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

        
        #region BindableProperty TitleIsVisible
        /// <summary>
        /// TitleIsVisible de la propiedad bindable
        /// </summary>
        private bool titleisvisible;
        public bool TitleIsVisible
        {
            get { return titleisvisible; }
            set { titleisvisible = value; OnPropertyChanged(); }
        }
        #endregion


    }
}
