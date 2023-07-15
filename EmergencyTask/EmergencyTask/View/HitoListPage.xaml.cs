using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HitoListPage : ContentPage
    {
        public HitoListPage()
        {
            InitializeComponent();
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