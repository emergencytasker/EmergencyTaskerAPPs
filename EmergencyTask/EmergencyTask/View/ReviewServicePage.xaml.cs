using EmergencyTask.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewServicePage : ContentPage
    {
        public ReviewServicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (BindingContext is ReviewServiceViewModel model)
            {
                model.SetStartControl(StarReview);
            }
            base.OnAppearing();
        }
    }
}