using EmergencyTask.Model;
using System.Collections.ObjectModel;

namespace EmergencyTask.ViewModel
{
    public class DireccionViewModel : ViewModelBase
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

        public DireccionViewModel()
        {
            Cartas = new ObservableCollection<CartaModel>
            {
                new CartaModel
                {
                    Image = "Icon.png",
                    Title = "Nombre de la direccion",
                    Subtitle = "Direccion donde se va a realizar el servicio",
                },

                new CartaModel
                {
                    Image = "Icon.png",
                    Title = "Nombre de la direccion",
                    Subtitle = "Direccion donde se va a realizar el servicio",
                },

                new CartaModel
                {
                    Image = "Icon.png",
                    Title = "Nombre de la direccion",
                    Subtitle = "Direccion donde se va a realizar el servicio",
                },

            };
        }
    }
}
