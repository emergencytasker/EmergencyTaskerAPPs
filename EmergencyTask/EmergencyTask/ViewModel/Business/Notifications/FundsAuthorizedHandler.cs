using EmergencyTask.Model;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel.Business
{
    public class FundsAuthorizedHandler : INotificationActionHandler
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
