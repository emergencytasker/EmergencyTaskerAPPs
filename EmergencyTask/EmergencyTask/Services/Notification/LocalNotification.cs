using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plugin.Notification
{
    public class LocalNotification
    {
        private ILocalNotification CrossLocalNotification { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool Sound { get; set; }
        public bool Vibrate { get; set; }
        public int Id { get; set; } = 23124;
        public IDictionary<string, string> Data { get; set; }
        public string Channel { get; set; }
        public bool Ongoing { get; set; }
        public DateTime? Date { get; set; }

        public LocalNotification()
        {
            CrossLocalNotification = DependencyService.Get<ILocalNotification>();
        }

        public bool Hide()
        {
            return CrossLocalNotification.Hide(Id);
        }

        public bool Show()
        {
            return CrossLocalNotification.Show(this);
        }

        public bool Update()
        {
            return CrossLocalNotification.Update(this);
        }

        private static ILocalNotificationHandler _localnotificationhalnder;
        public static void SetHandler(ILocalNotificationHandler localNotificationHandler)
        {
            _localnotificationhalnder = localNotificationHandler;
        }

        public static async Task<Dictionary<string,string>> OnOpen(Dictionary<string, string> parameters)
        {
            var service = DependencyService.Get<ILocalNotification>();
            return await _localnotificationhalnder?.Open(service, parameters);
        }
    }
}
