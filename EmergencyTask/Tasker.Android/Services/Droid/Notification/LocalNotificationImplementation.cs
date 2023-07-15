using Android.App;
using Android.Content;
using Android.Support.V4.App;
using LightForms;
using Plugin.Droid.Notification;
using Plugin.Notification;
using System;
using System.Collections.Generic;
using Tasker.Android;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotificationImplementation))]
namespace Plugin.Droid.Notification
{
    public class LocalNotificationImplementation : ILocalNotification
    {

        /// <summary>
        /// Contexto de la app
        /// </summary>
        private static Context _context;

        /// <summary>
        /// Envia una notificacion
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public bool Show(LocalNotification notification)
        {
            try
            {
                var notificationmanagercompat = NotificationManagerCompat.From(_context);
                if (notificationmanagercompat == null) return false;
                if (!notificationmanagercompat.AreNotificationsEnabled()) return false;
                var droidnotification = GetNotification(notification);
                if (droidnotification == null) return false;
                notificationmanagercompat.Notify(notification.Id, droidnotification);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Devuelve una notificacion de android
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        private Android.App.Notification GetNotification(LocalNotification notification)
        {
            bool channelset = false;
            NotificationChannel channel = null;

            try
            {
                var importance = NotificationImportance.High;
                channel = new NotificationChannel($"{notification.GetHashCode()}", notification.Channel, importance);
                channelset = true;
            }
            catch
            {

            }

            var activityintent = new Intent(_context, typeof(MainActivity));
            activityintent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            if (notification.Data != null)
            {
                var kayvalues = new List<string>();
                foreach (var keyvalue in notification.Data)
                {
                    kayvalues.Add($"{keyvalue.Key},{keyvalue.Value}");
                }
                activityintent.PutExtra("keyvalue", string.Join(",", kayvalues));
            }

            var stackbuilder = Android.App.TaskStackBuilder.Create(_context);
            PendingIntent pendingintent = null;

            try
            {
                stackbuilder.AddNextIntentWithParentStack(activityintent);
                pendingintent = stackbuilder.GetPendingIntent(notification.Id, PendingIntentFlags.UpdateCurrent);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            var droidnotificationbuilder = new NotificationCompat.Builder(_context, notification.Channel)
                        .SetContentTitle(notification.Title)
                        .SetContentText(notification.Message)
                        .SetSmallIcon(Resource.Drawable.icon)
                        .SetOngoing(notification.Ongoing)
                        .SetAutoCancel(false)
                        .SetOnlyAlertOnce(notification.Ongoing)
                        .SetChannelId($"{notification.GetHashCode()}");

            droidnotificationbuilder.SetDefaults((int)NotificationDefaults.All);

            if (notification.Sound && notification.Vibrate)
            {
                try
                {
                    var sounduri = Android.Net.Uri.Parse(ContentResolver.SchemeAndroidResource + "://" + _context.PackageName + "/" + Resource.Raw.notification);
                    droidnotificationbuilder.SetSound(sounduri);
                }
                catch { }
                droidnotificationbuilder.SetVibrate(new long[] { 10, 20, 10, 20 });
            }
            else
            {
                if (notification.Sound)
                {
                    try
                    {
                        var sounduri = Android.Net.Uri.Parse(ContentResolver.SchemeAndroidResource + "://" + _context.PackageName + "/" + Resource.Raw.notification);
                        droidnotificationbuilder.SetSound(sounduri);
                    }
                    catch { }
                }

                if (notification.Vibrate)
                {
                    droidnotificationbuilder.SetVibrate(new long[] { 10, 20, 10, 20 });
                }
            }

            if (notification.Date.HasValue)
            {
                droidnotificationbuilder
                    .SetShowWhen(true)
                    .SetWhen(NotifyTimeInMilliseconds(notification.Date.Value));
            }

            if (pendingintent != null) droidnotificationbuilder.SetContentIntent(pendingintent);

            var droidnotification = droidnotificationbuilder.Build();

            NotificationManager manager = (NotificationManager)_context.GetSystemService(Context.NotificationService);
            if (manager == null) return null;
            try
            {
                if (channelset) manager.CreateNotificationChannel(channel);
            }
            catch (Exception ex)
            {

            }
            return droidnotification;
        }

        /// <summary>
        /// Convert DateTime To Millis
        /// </summary>
        /// <param name="notifyTime"></param>
        /// <returns></returns>
        private long NotifyTimeInMilliseconds(DateTime notifyTime)
        {
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            var epochDifference = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            var utcAlarmTimeInMillis = utcTime.AddSeconds(-epochDifference).Ticks / 10000;
            return utcAlarmTimeInMillis;
        }

        /// <summary>
        /// Oculta una notificacion por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Hide(int id)
        {
            var notificationmanagercompat = NotificationManagerCompat.From(_context);
            if (notificationmanagercompat == null) return false;
            notificationmanagercompat.Cancel(id);
            return true;
        }

        /// <summary>
        /// Inicializa el plugin de notificaciones
        /// </summary>
        /// <param name="context"></param>
        public static void Init(Context context)
        {
            _context = context;
            DependencyService.Register<LocalNotificationImplementation>();
        }

        /// <summary>
        /// Actualiza una notificacion, en caso de no existir es lanzada nuevamente
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public bool Update(LocalNotification notification)
        {
            return Show(notification);
        }
    }
}