using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel.Business
{
    public class ChatHandler : INotificationActionHandler
    {
        public async Task Execute(NotificationModel model, NotificationData data)
        {
            await Task.Delay(1);
            var perfil = (Application.Current as App).Perfil;
            var me = Usuario.GetUserLogin();
            var page = new ContentPage();
            var title = "Home";

            if (me == null)
            {
                page = perfil == API.Enum.Perfil.Client ? (ContentPage) new HomePage() : new WaitingServicePage() { BindingContext = new WaitingServiceViewModel() };
            }
            else
            {
                var id = data.id;
                var idcliente = 0;
                var idtrabajador = 0;
                if(me.Perfil == API.Enum.Perfil.Client)
                {
                    idcliente = me.id;
                    idtrabajador = id;
                }
                else
                {
                    idtrabajador = me.id;
                    idcliente = id;
                }
                page = new ChatPage(idcliente, idtrabajador);
                title = model.Title;
            }

            var menu = new HamburgerMenuPage
            {
                Detail = new NavigationPage(page)
                {
                    Title = title,
                    BarBackgroundColor = (Color)Application.Current.Resources["Accent"],
                    BarTextColor = Color.White
                }
            };

            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = menu;
            });
        }
    }
}