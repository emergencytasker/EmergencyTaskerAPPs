using EmergencyTask.API;
using EmergencyTask.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class LogoutViewModel : ViewModelBase
    {
        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            var me = Usuario.GetUserLogin();
            if (me != null)
            {
                try
                {
                    var sessionid = await SecureStorage.GetAsync("sessionid");
                    await Client.LogOut(sessionid, me.id);
                }
                catch { }
            }
            Usuario.Delete();
            App.Restart();
        }
    }
}