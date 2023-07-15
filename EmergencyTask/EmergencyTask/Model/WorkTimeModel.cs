using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyTask.Model
{
    public class WorkTimeModel : ModelBase
    {

        #region BindableProperty Inicio
        /// <summary>
        /// Inicio de la propiedad bindable
        /// </summary>
        private DateTime inicio;
        public DateTime Inicio
        {
            get { return inicio; }
            set { inicio = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty Fin
        /// <summary>
        /// Fin de la propiedad bindable
        /// </summary>
        private DateTime fin;
        public DateTime Fin
        {
            get { return fin; }
            set { fin = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty Tiempo
        /// <summary>
        /// Tiempo de la propiedad bindable
        /// </summary>
        private TimeSpan tiempo;
        public TimeSpan Tiempo
        {
            get { return tiempo; }
            set { tiempo = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty Total
        /// <summary>
        /// Total de la propiedad bindable
        /// </summary>
        private double total;
        public double Total
        {
            get { return total; }
            set { total = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Tarifa
        /// <summary>
        /// Tarifa
        /// </summary>
        private double tarifa;
        public double Tarifa
        {
            get { return tarifa; }
            set { tarifa = value; OnPropertyChanged(); }
        }
        #endregion

    }
}
