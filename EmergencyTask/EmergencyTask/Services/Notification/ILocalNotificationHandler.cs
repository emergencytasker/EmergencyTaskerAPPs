using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Notification
{
    public interface ILocalNotificationHandler
    {

        Task<Dictionary<string,string>> Open(ILocalNotification service, Dictionary<string,string> data);

    }
}