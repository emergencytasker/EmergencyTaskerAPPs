using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectionPage : ContentPage
    {
        public DirectionPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(BindingContext is ViewModel.DirectionViewModel viewmodel)
            {
                viewmodel.SetMapa(Mapa);
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}