using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class CategoriaViewModel : ViewModelBase
    {

        #region BindableProperty Carta
        /// <summary>
        /// Carta de la propiedad bindable
        /// </summary>
        private CartaModel carta;
        public CartaModel Carta
        {
            get { return carta; }
            set
            {
                carta = value;
                OnPropertyChanged();
                if (value == null) return;
                value.Action?.Execute(value);
            }
        }
        #endregion

        #region BindableProperty Cartas
        /// <summary>
        /// Carta de la propiedad bindable
        /// </summary>
        private ObservableCollection<CartaModel> cartas;
        public ObservableCollection<CartaModel> Cartas
        {
            get { return cartas; }
            set { cartas = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Search
        /// <summary>
        /// Search
        /// </summary>
        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged(); OnSearchChanged(value); }
        }

        private void OnSearchChanged(string value)
        {
            Debug.WriteLine($"[OnSearchChanged] {value}");
            if (string.IsNullOrEmpty(value))
            {
                SetSource(Source);
            }
            else
            {
                if (Source != null)
                {
                    var source = Source.Where(s => !string.IsNullOrEmpty(s.Title) && s.Title.ToLower().Contains(value));
                    SetSource(source);
                }
            }
        }
        #endregion

        public int IdCategory { get; set; }
        public string Category { get; set; }
        public IEnumerable<CartaModel> Source { get; set; }
        IEnumerable<CartaModel> subcategories { get; set; }
        public IEnumerable<API.Response.Service> Services { get; set; }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            Cartas = new ObservableCollection<CartaModel>();
            subcategories = new ObservableCollection<CartaModel>();
            IsBusy = true;

            if (Services != null)
            {
                subcategories = Services.Where(s => s.idcategoria == IdCategory).Select(s => new CartaModel
                {
                    Id = s.idsubcategoria,
                    Action = new ExtendCommand(Action_Clicked, new InternetValidator()),
                    Image = Client.Path(s.imagensubcategoria),
                    Title = s.subcategoria
                });

                if (Source == null)
                {
                    Source = new ObservableCollection<CartaModel>(subcategories);
                }
            }

            SetSource(subcategories);
            IsBusy = false;
        }

        private void SetSource(IEnumerable<CartaModel> subcategories)
        {
            if (subcategories != null) Cartas = new ObservableCollection<CartaModel>(subcategories);
            else Cartas = new ObservableCollection<CartaModel>();
        }

        private async void Action_Clicked(object arg1, IExecuteValidator[] validators)
        {
            if (Carta == null) return;
            IsBusy = true;
            await Navigation.PushAsync(new DirectionPage
            {
                BindingContext = new DirectionViewModel
                {
                    Service = new ServiceModel
                    {
                        IdCategoria = IdCategory,
                        Category = Category,
                        IdSubcategory = Carta.Id,
                        SubCategory = Carta.Title
                    }
                }
            });
            Carta = null;
            IsBusy = false;
        }
    }
}