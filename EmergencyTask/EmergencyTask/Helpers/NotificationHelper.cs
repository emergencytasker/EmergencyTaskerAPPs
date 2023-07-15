using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Business;
using Plugin.FirebasePushNotification;
using Plugin.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.Helpers
{
    public class NotificationHelper : ILocalNotificationHandler
    {
        public static bool NotificationReceived(string title, string message, IDictionary<string, string> customData)
        {
            return Show(title, message, customData);
        }

        static NotificationHelper()
        {
            LocalNotification.SetHandler(new NotificationHelper());
        }

        public async Task<Dictionary<string, string>> Open(ILocalNotification service, Dictionary<string, string> data)
        {
            if (data == null || data.Count <= 0) return null;
            if (!data.ContainsKey("idaction") || !data.ContainsKey("id")) return null;
            if (!int.TryParse(data["idaction"], out int action))  return null;
            if (!int.TryParse(data["id"], out int id)) return null;
            var idaction = (NotificationActions)action;
            var notificationdata = new NotificationData
            {
                id = id,
                idaction = action
            };
            var model = new NotificationModel
            {
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(data),
                IdAction = action,
                Id = 0,
                Title = "Emergency Tasker",
                Subtitle = "Notification",
                HoraFecha = DateTime.Now.ToShortDateString()
            };
            var factory = new EnumFactory<NotificationActions, INotificationActionHandler>("EmergencyTask.ViewModel.Business.", "Handler");
            var handler = factory.Resolve(idaction);
            if (handler == null) return null;
            await handler?.Execute(model, notificationdata);
            return data;
        }

        public static void Init()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
            CrossFirebasePushNotification.Current.OnNotificationAction += Current_OnNotificationAction;
            CrossFirebasePushNotification.Current.OnNotificationDeleted += Current_OnNotificationDeleted;
            CrossFirebasePushNotification.Current.OnNotificationError += Current_OnNotificationError;
            CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened;
        }

        private static async void Current_OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs e)
        {
            var data = GetNotification(e.Data);
            if (data == null) return;
            await LocalNotification.OnOpen(data);
        }

        private static Dictionary<string,string> GetNotification(IDictionary<string, object> data)
        {
            if (data == null) return null;
            var dicstr = new Dictionary<string, string>();
            foreach (var item in data)
            {
                dicstr.Add(item.Key, item.Value.ToString());
            }
            return dicstr;
        }

        private static void Current_OnNotificationError(object source, FirebasePushNotificationErrorEventArgs e)
        {

        }

        private static void Current_OnNotificationDeleted(object source, FirebasePushNotificationDataEventArgs e)
        {

        }

        private static void Current_OnNotificationAction(object source, FirebasePushNotificationResponseEventArgs e)
        {

        }

        private static void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Token, "FireBase");
            if (string.IsNullOrEmpty(e.Token)) return;
            var variable = DataBase.GetVariable();
            variable.AppCenterId = e.Token;
            DataBase.SetVariable(variable);
        }

        private void GoToNotifications()
        {
            Application.Current.MainPage = new HamburgerMenuPage
            {
                Detail = new NavigationPage(new NotificationPage())
                {
                    Title = AppResource.Notificaciones,
                    BarBackgroundColor = (Color)Application.Current.Resources["Accent"],
                    BarTextColor = Color.White
                }
            };
        }

        /// <summary>
        /// Registra en la base de datos al usuario para recibir notificaciones
        /// </summary>
        /// <param name="idusuario"></param>
        /// <returns></returns>
        public static async Task<bool> RegisterForNotifications(int idusuario, string appcenterid)
        {
            if (!string.IsNullOrEmpty(appcenterid))
            {
                var appcenter = (await Client.AppCenter.Query(new AppCenter
                {
                    idusuario = idusuario,
                    appcenterid = appcenterid,
                    platform = Device.RuntimePlatform
                }) ?? new List<AppCenter>()).FirstOrDefault();
                if (appcenter != null)
                {
                    appcenter = await Client.AppCenter.Update(appcenter.id, new Dictionary<string, string>
                    {
                        { nameof(AppCenter.appcenterid), appcenterid }
                    });
                }
                else
                {
                    appcenter = await Client.AppCenter.Add(new AppCenter
                    {
                        appcenterid = appcenterid,
                        idusuario = idusuario,
                        platform = Device.RuntimePlatform
                    });
                }
                return appcenter != null && appcenter.id > 0;
            }
            return false;
        }

        public static bool Show(string title, string message, IDictionary<string, string> customdata = null)
        {
            if (customdata == null) customdata = new Dictionary<string, string>();

            title = title ?? "-----";
            message = message ?? "-----";

            if(customdata != null && customdata.Count == 0)
            {
                customdata.Add("title", title);
                customdata.Add("message", message);
            }

            LocalNotification localnotification = new LocalNotification
            {
                Title = title,
                Message = message,
                Channel = "defaultchannel",
                Data = customdata,
                Sound = true,
                Vibrate = true,
                Id = customdata.GetHashCode()
            };

            return localnotification.Show();
        }

        public static void Stop()
        {
            CrossFirebasePushNotification.Current.OnNotificationReceived -= Current_OnNotificationReceived;
        }

        private static void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            if (e == null) return;
            if (e.Data == null) return;
            if (!e.Data.ContainsKey("body") || !e.Data.ContainsKey("title")) return;
            var body = e.Data["body"].ToString();
            var title = e.Data["title"].ToString();
            var data = new Dictionary<string, string>();
            foreach (var item in e.Data)
            {
                data.Add(item.Key, item.Value.ToString());
            }
            NotificationReceived(title, body, data);
        }

        public static void Start()
        {
            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
        }
    }
}
