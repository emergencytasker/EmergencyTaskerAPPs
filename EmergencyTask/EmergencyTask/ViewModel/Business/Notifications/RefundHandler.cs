using System.Threading.Tasks;
using EmergencyTask.Model;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel.Business
{
    public class RefundHandler : INotificationActionHandler
    {
        public async Task Execute(NotificationModel model, NotificationData data)
        {
            if (Application.Current.MainPage.BindingContext is ViewModelBase viewmodel)
            {
                await viewmodel.Navigation.PushAsync(new HitoListPage
                {
                    BindingContext = new HitoListViewModel(data.id)
                });
            }
        }
    }
}
