using EmergencyTask.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenuPage : MasterDetailPage
    {
        public HamburgerMenuPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(BindingContext is HamburguerMenuViewModel viewmodel)
            {
                viewmodel.SetMasterDetailPage(this);
            }
        }
    }
}