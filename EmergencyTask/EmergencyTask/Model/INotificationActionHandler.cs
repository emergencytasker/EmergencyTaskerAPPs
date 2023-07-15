using System.Threading.Tasks;

namespace EmergencyTask.Model
{
    public interface INotificationActionHandler
    {
        Task Execute(NotificationModel model, NotificationData data);
    }
}