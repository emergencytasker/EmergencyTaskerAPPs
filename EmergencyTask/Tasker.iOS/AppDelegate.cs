using Acr.UserDialogs;
using EmergencyTask;
using FFImageLoading.Forms.Platform;
using Foundation;
using Plugin.FirebasePushNotification;
using Plugin.iOS.Notification;
using Rg.Plugins.Popup;
using System;
using System.Diagnostics;
using UIKit;
using Xamarin;
using Xamarin.Forms;

namespace Tasker.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            Forms.SetFlags("CollectionView_Experimental", "IndicatorView_Experimental");
            CachedImageRenderer.Init();
            FormsGoogleMaps.Init("AIzaSyAPPN7LeRWoesP8JZfqPWjthq2T7Zu1mxg");
            LocalNotificationImplementation.Init(app, options, Window);
            Popup.Init();
            Forms.Init();
            LoadApplication(new App(EmergencyTask.API.Enum.Perfil.Tasker));
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
            System.Diagnostics.Debug.WriteLine(userInfo);
            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}


