using EmergencyTask.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileClientPage : ContentPage
    {
        public ProfileClientPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if(BindingContext is ProfileClientViewModel viewmodel)
            {
                viewmodel.SetStars(Stars);
            }
            base.OnAppearing();
        }
    }
}