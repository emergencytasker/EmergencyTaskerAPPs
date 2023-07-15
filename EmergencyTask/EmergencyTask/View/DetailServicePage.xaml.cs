using EmergencyTask.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailServicePage : ContentPage
    {
        public DetailServicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (!(BindingContext is DetailServiceViewModel model)) return;
            model.SetMapa(Mapa);
            model.SetStars(Stars);
            base.OnAppearing();
        }
    }
}