using System.Threading.Tasks;
using EmergencyTask.Model;
using EmergencyTask.Strings;

namespace EmergencyTask.ViewModel.Business
{
    public class DefaultHandler : INotificationActionHandler
    {
        public async Task Execute(NotificationModel model, NotificationData data)
        {
            await App.Current.MainPage.DisplayAlert(model.Title, model.Subtitle, AppResource.Aceptar);
        }
    }
}