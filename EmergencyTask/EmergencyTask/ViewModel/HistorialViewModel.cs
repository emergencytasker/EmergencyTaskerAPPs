using EmergencyTask.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EmergencyTask.ViewModel
{
    public class HistorialViewModel : ViewModelBase
    {

        #region BindableProperty Carta
        /// <summary>
        /// Carta de la propiedad bindable
        /// </summary>
        private CartaModel carta;
        public CartaModel Carta
        {
            get { return carta; }
            set { carta = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Cartas
        /// <summary>
        /// Cartas de la propiedad bindable
        /// </summary>
        private ObservableCollection<CartaModel> cartas;
        public ObservableCollection<CartaModel> Cartas
        {
            get { return cartas; }
            set { cartas = value; OnPropertyChanged(); }
        }
        #endregion

        public HistorialViewModel()
        {
            Cartas = new ObservableCollection<CartaModel>
            {
                new CartaModel
                {
                    Image = "Icon.png",
                    Title = "Servicio",
                    Subtitle = "Tarea a Realizar",
                    HoraFecha = "Hora y dia que se realiza",
                    Asistente = "Quien realiza la tarea",
                },

                new CartaModel
                {
                    Image = "Icon.png",
                    Title = "Servicio",
                    Subtitle = "Tarea a Realizar",
                    HoraFecha = "Hora y dia que se realiza",
                    Asistente = "Quien realiza la tarea",
                },

                new CartaModel
                {
                    Image = "Icon.png",
                    Title = "Servicio",
                    Subtitle = "Tarea a Realizar",
                    HoraFecha = "Hora y dia que se realiza",
                    Asistente = "Quien realiza la tarea",
                },

            };
        }
    }
}
