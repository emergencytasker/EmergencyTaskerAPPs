using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Firebase;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;

namespace Tasker.Android
{
#if DEBUG
    [Application(Debuggable = true)]
#else
	[Application(Debuggable = false)]
#endif
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer): base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);

            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }
#if DEBUG
            //FirebasePushNotificationManager.Initialize(this, true);
#else
            //FirebasePushNotificationManager.Initialize(this, false);
#endif
        }
    }
}