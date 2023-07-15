using EmergencyTask.Strings;
using System;
using System.Threading;

namespace EmergencyTask.Helpers
{
    public static class DateTimeHelper
    {

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private const string DateFormat = "yyyy-MM-dd";

        public static string ToMySqlDateTimeFormat(this DateTime datetime)
        {
            return datetime.ToString(DateTimeFormat);
        }

        public static DateTime FromMySqlDateTimeFormat(this string datetime)
        {
            try
            {
                return DateTime.Parse(datetime);
            }
            catch(Exception ex) { Microsoft.AppCenter.Crashes.Crashes.TrackError(ex); }
            return DateTime.Now;
        }

        public static string ToMySqlDateFormat(this DateTime date)
        {
            return date.ToString(DateFormat);
        }

        public static DateTime FromMySqlDateFormat(this string date)
        {
            try
            {
                return DateTime.Parse(date);
            }
            catch { }
            return new DateTime();
        }

        /// <summary>
        /// Devuelve un formato para el usuario
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToPrettyDate(this DateTime datetime)
        {
            TimeSpan s = DateTime.Now.Subtract(datetime);

            int dayDiff = (int)s.TotalDays;

            int secDiff = (int)s.TotalSeconds;

            if (dayDiff < 0 || dayDiff >= 31) return datetime.ToString(AppResource.PrettyDateFormat, Thread.CurrentThread.CurrentUICulture);

            if (dayDiff == 0)
            {
                if (secDiff < 60) return AppResource.JustoAhora;

                if (secDiff < 3600) return string.Format(AppResource.HaceXMinutos,
                        Math.Floor((double)secDiff / 60));

                if (secDiff < 86400) return string.Format(AppResource.HaceXHoras,
                        Math.Floor((double)secDiff / 3600));
            }

            if (dayDiff == 1) return AppResource.Ayer;

            return datetime.ToString("MM/dd/yyyy", Thread.CurrentThread.CurrentUICulture);
        }
    }
}