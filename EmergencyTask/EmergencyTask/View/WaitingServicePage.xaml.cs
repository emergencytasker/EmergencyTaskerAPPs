using EmergencyTask.ViewModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitingServicePage : ContentPage
    {
        public WaitingServicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (BindingContext is WaitingServiceViewModel viewmodel)
            {
                viewmodel.SetMapa(Mapa);
            }
            base.OnAppearing();
        }
    }
}