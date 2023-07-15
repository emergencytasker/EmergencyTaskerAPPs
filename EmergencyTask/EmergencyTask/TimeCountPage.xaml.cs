using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeCountPage : ContentPage
    {
        public TimeCountPage()
        {
            InitializeComponent();
        }

        public TimeSpan Cronometro { get; private set; }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CodeValidationTaskPopup());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}