using EmergencyTask.ViewModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (BindingContext is MainViewModel model)
            {
                model.SetCircles(Circles.Children.Cast<BoxView>());
            }
        }
    }
}
