using FFImageLoading.Forms.Platform;
using Foundation;
using Plugin.FirebasePushNotification;
using Plugin.iOS.Notification;
using Rg.Plugins.Popup;
using System;
using UIKit;
using Xamarin;
using Xamarin.Forms;

namespace EmergencyTask.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("CollectionView_Experimental", "IndicatorView_Experimental");
            CachedImageRenderer.Init();
            FormsGoogleMaps.Init("AIzaSyAPPN7LeRWoesP8JZfqPWjthq2T7Zu1mxg");
            LocalNotificationImplementation.Init(app, options, Window);
            Forms.Init();
            Popup.Init();
            LoadApplication(new App(API.Enum.Perfil.Client));
            FirebasePushNotificationManager.Initialize(options, true);
            Plugin.Tick.TickImplementation.Init();
            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// Cuando se recibe una notificacion
        /// </summary>
        /// <param name="application"></param>
        /// <param name="notification"></param>
        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            LocalNotificationImplementation.ReceivedLocalNotification(application, notification, Window);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        }

        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}
