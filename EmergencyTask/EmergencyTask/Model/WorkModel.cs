using EmergencyTask.Helpers;
using System;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class WorkModel : ModelBase
    {
        public int Id { get; set; }

        #region BindableProperty Empresa
        /// <summary>
        /// Empresa de la propiedad bindable
        /// </summary>
        private string empresa;
        public string Empresa
        {
            get { return empresa; }
            set { empresa = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Puesto
        /// <summary>
        /// Puesto de la propiedad bindable
        /// </summary>
        private string puesto;
        public string Puesto
        {
            get { return puesto; }
            set { puesto = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Inicio
        /// <summary>
        /// Inicio
        /// </summary>
        private DateTime inicio;
        public DateTime Inicio
        {
            get { return inicio; }
            set { inicio = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Fin
        /// <summary>
        /// Fin
        /// </summary>
        private DateTime fin;
        public DateTime Fin
        {
            get { return fin; }
            set { fin = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Description
        /// <summary>
        /// Description de la propiedad bindable
        /// </summary>
        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property TapOpciones
        /// <summary>
        /// TapOpciones
        /// </summary>
        private Command tapopciones;
        public Command TapOpciones
        {
            get { return tapopciones; }
            set { tapopciones = value; OnPropertyChanged(); }
        }
        #endregion

        public string FechaInicio
        {
            get
            {
                return Inicio.ToMySqlDateFormat();
            }
        }

        public string FechaFin
        {
            get
            {
                return Fin.ToMySqlDateFormat();
            }
        }
    }
}