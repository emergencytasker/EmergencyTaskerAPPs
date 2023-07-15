using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Xamarin.Forms;
using Acr.UserDialogs;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using EmergencyTask;
using EmergencyTask.API.Enum;
using Rg.Plugins.Popup;
using Xamarin;
using Plugin.Droid.Notification;
using FFImageLoading.Forms.Platform;
using Plugin.FirebasePushNotification;
using System.Collections.Generic;
using Android.Content;
using System.Diagnostics;
using Android.OS;
using Firebase;

namespace Tasker.Android
{
    [Activity(Label = "Tasker", Icon = "@drawable/eticon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Forms.SetFlags("CollectionView_Experimental", "IndicatorView_Experimental");
            UserDialogs.Init(this);
            CachedImageRenderer.Init(true);
            LocalNotificationImplementation.Init(this);
            Popup.Init(this);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsGoogleMaps.Init(this, savedInstanceState);
            // AnimationViewRenderer.Init();
            var parameters = GetNotification();
            LoadApplication(new App(Perfil.Tasker, parameters));
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            Plugin.Tick.TickImplementation.Init();
        }

        private Dictionary<string,string> GetNotification()
        {
            if (Intent.Extras == null) return null;
            var keyvalue = Intent.Extras.GetString("keyvalue", "");
            if (string.IsNullOrEmpty(keyvalue)) return null;
            var keyvalues = keyvalue.Split(",");
            if (keyvalues.Length == 0) return null;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            for (int i = 0; i < keyvalues.Length; i += 2)
            {
                var key = keyvalues[i];
                var value = keyvalues[i + 1];
                parameters.Add(key, value);
            }
            return parameters;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (Popup.SendBackPressed(base.OnBackPressed))
            {

            }
            else
            {

            }
        }

    }
}