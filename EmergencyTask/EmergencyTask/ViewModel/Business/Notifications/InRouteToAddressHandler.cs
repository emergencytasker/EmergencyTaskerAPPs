using System.Threading.Tasks;
using EmergencyTask.Model;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel.Business
{
    public class InRouteToAddressHandler : INotificationActionHandler
    {
        public async Task Execute(NotificationModel model, NotificationData data)
        {
            if (Application.Current.MainPage.BindingContext is ViewModelBase viewmodel)
            {
                await viewmodel.GoToServiceDetailPage(data.id);
            }
        }
    }
}