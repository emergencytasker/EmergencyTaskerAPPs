using Foundation;
using Plugin.iOS.Notification;
using Plugin.Notification;
using System.Collections.Generic;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotificationImplementation))]
namespace Plugin.iOS.Notification
{
    public class LocalNotificationImplementation : ILocalNotification
    {

        /// <summary>
        /// Inicializa el plugin de notificaciones
        /// </summary>
        /// <param name="application"></param>
        /// <param name="options"></param>
        /// <param name="window"></param>
        public static void Init(UIApplication app, NSDictionary options, UIWindow window)
        {
            var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
            );
            app.RegisterUserNotificationSettings(notificationSettings);

            if (options != null)
            {
                if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    var localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
                    if (localNotification != null)
                    {
                        UIAlertController okayAlertController = UIAlertController.Create(localNotification.AlertAction, localNotification.AlertBody, UIAlertControllerStyle.Alert);
                        okayAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, null));
                        okayAlertController.AddAction(UIAlertAction.Create("View", UIAlertActionStyle.Default, new System.Action<UIAlertAction>((alert) =>
                        {
                            var dic = new Dictionary<string, string>();
                            try
                            {
                                var keys = options.Keys;
                                var values = options.Values;
                                for (int i = 0; i < keys.Length; i++)
                                {
                                    dic.Add(keys[i].ToString(), values[i].ToString());
                                }
                            }
                            catch { }
                            LocalNotification.OnOpen(dic);
                        })));
                        window.RootViewController.PresentViewController(okayAlertController, true, null);
                        // reset our badge
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                    }
                }
            }

            DependencyService.Register<LocalNotificationImplementation>();
        }

        /// <summary>
        /// Envia una notificacion
        /// </summary>
        /// <param name="localnotification"></param>
        /// <returns></returns>
        public bool Show(LocalNotification localnotification)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UILocalNotification notification = new UILocalNotification
                {
                    FireDate = NSDate.Now.AddSeconds(1),
                    AlertAction = localnotification.Title,
                    AlertBody = localnotification.Message,
                    ApplicationIconBadgeNumber = 1
                };
                if (localnotification.Sound)
                {
                    notification.SoundName = "notification.mp3";
                }
                UIApplication.SharedApplication.ScheduleLocalNotification(notification);
            });
            return true;
        }

        public static void ReceivedLocalNotification(UIApplication application, UILocalNotification notification, UIWindow window)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                if (notification != null)
                {
                    notification.FireDate = NSDate.Now.AddSeconds(1);
                    notification.SoundName = "notification.mp3";
                    UIApplication.SharedApplication.ScheduleLocalNotification(notification);
                }
                /*
                UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
                okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                window.RootViewController.PresentViewController(okayAlertController, true, null);
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                */
            });
        }

        /// <summary>
        /// Oculta una notificacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Hide(int id)
        {
            var requests = new string[] { id.ToString() };
            UNUserNotificationCenter.Current.RemoveDeliveredNotifications(requests);
            return true;
        }

        public bool Update(LocalNotification notification)
        {
            return true;
        }
    }
}