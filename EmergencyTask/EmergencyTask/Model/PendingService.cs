using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class PendingService : ModelBase
    {
        public int Id { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public string Cliente { get; set; }
        public double Calificacion { get; set; }
        public string Imagen { get; set; }
        public string Background { get; set; }

        #region Notified Property Distancia
        /// <summary>
        /// Distancia
        /// </summary>
        private string distancia;
        public string Distancia
        {
            get { return distancia; }
            set { distancia = value; OnPropertyChanged(); }
        }
        #endregion


        public ICommand Aceptar { get; set; }
        public ICommand Cancelar { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Direccion { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string DateFormat { get; set; }

        #region Notified Property ServiceSelected
        /// <summary>
        /// ServiceSelected
        /// </summary>
        private ICommand gotoinfo;
        public ICommand GoToInfo
        {
            get { return gotoinfo; }
            set { gotoinfo = value; OnPropertyChanged(); }
        }

        public string Descripcion { get; internal set; }
        #endregion
    }
}