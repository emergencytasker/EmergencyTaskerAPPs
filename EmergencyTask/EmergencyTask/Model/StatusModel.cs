using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class StatusModel : ModelBase
    {

        #region BindableProperty Action
        /// <summary>
        /// Action de la propiedad bindable
        /// </summary>
        private Command action;
        public Command Action
        {
            get { return action; }
            set { action = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Status
        /// <summary>
        /// Status de la propiedad bindable
        /// </summary>
        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty Fecha
        /// <summary>
        /// Fecha de la propiedad bindable
        /// </summary>
        private string fecha;
        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty Hora
        /// <summary>
        /// Hora de la propiedad bindable
        /// </summary>
        private string hora;
        public string Hora
        {
            get { return hora; }
            set { hora = value; OnPropertyChanged(); }
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


        #region BindableProperty Icon
        /// <summary>
        /// icon de la propiedad bindable
        /// </summary>
        private string icon;
        public string Icon
        {
            get { return icon; }
            set { icon = value; OnPropertyChanged(); }
        }
        #endregion

    }
}
