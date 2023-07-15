using EmergencyTask.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceMapPage : ContentPage
    {
        public ServiceMapPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (BindingContext is ServiceMapVieWModel model)
            {
                model.SetMapa(Mapa);
            }
            base.OnAppearing();
        }
    }
}