using EmergencyTask.Model;
using Rg.Plugins.Popup.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class AccesoriesListViewModel : ViewModelBase
    {

        #region Notified Property AgregarAccesorio
        /// <summary>
        /// AgregarAccesorio
        /// </summary>
        private Command agregaraccesorio;
        public Command AgregarAccesorio
        {
            get { return agregaraccesorio; }
            set { agregaraccesorio = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Accesorios
        /// <summary>
        /// Accesorios de la propiedad bindable
        /// </summary>
        private ObservableCollection<AccesorioModel> accesorios;
        public ObservableCollection<AccesorioModel> Accesorios
        {
            get { return accesorios; }
            set { accesorios = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Continuar
        /// <summary>
        /// Continuar
        /// </summary>
        private Command continuar;
        public Command Continuar
        {
            get { return continuar; }
            set { continuar = value; OnPropertyChanged(); }
        }
        #endregion

        private ServiceModel Service;

        public AccesoriesListViewModel(ServiceModel service)
        {
            Service = service;
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            Accesorios = new ObservableCollection<AccesorioModel>();
            AgregarAccesorio = new Command(AgregarAccesorio_Clicked);
            Continuar = new Command(Continuar_Clicked);
        }

        private async void Continuar_Clicked(object obj)
        {
            Service.Accesorios = Accesorios.ToList();
            await Navigation.PushAsync(new CandidateListPage
            {
                BindingContext = new CandidateListViewModel(Service)
            });
        }

        private async void AgregarAccesorio_Clicked(object obj)
        {
            await Navigation.PushPopupAsync(new AgregarAccesorioPopup(async (model) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Accesorios.Add(new AccesorioModel
                    {
                        Cantidad = model.Cantidad,
                        Nombre = model.Nombre
                    });
                });
                await Navigation.PopPopupAsync();
            }));
        }

    }
}
