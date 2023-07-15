using ETClient.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ETClient
{
    public static class Extentions
    {
        public static string NormalizeBaseUrl(this string url)
        {
            string path_url = string.Empty;

            if (string.IsNullOrEmpty(url))
            {
                return path_url;
            }

            try
            {
                Uri base_url = new Uri(url);
                path_url = base_url.PathAndQuery;

                if (path_url.StartsWith("/"))
                {
                    path_url = path_url.Substring(1);
                }

                return path_url;
            }
            catch
            {
                path_url = url;
                if (path_url.StartsWith("/"))
                {
                    path_url = path_url.Substring(1);
                }
                return path_url;
            }
        }

        public static string MySQLEscape(this string str)
        {
            return Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
                delegate (Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":            // ASCII NUL (0x00) character
                            return "\\0";
                        case "\b":              // BACKSPACE character
                            return "\\b";
                        case "\n":              // NEWLINE (linefeed) character
                            return "\\n";
                        case "\r":              // CARRIAGE RETURN character
                            return "\\r";
                        case "\t":              // TAB
                            return "\\t";
                        case "\u001A":          // Ctrl-Z
                            return "\\Z";
                        default:
                            return "\\" + v;
                    }
                });
        }

        public static void WriteText(this MemoryStream stream, string text)
        {
            byte[] bytes = StringEncoder.EncodeToBytes(text);
            bytes = bytes.Concat(new byte[] { 0x0A }).ToArray();
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteFooter(this MemoryStream stream)
        {
            byte[] bytes = new byte[] { 0x0A, 0x0A, 0x0A, 0x0A };
            stream.Write(bytes, 0, bytes.Length);
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return base64EncodedData;
            }
        }

        public static string ClearStoreUrl(this string _value)
        {
            return Regex.Replace(_value, @"[^0-9A-Za-z ,]", "").Replace(" ", "_");
        }

        public static string GenerateStoreUrl(this string _value, int length = 10)
        {
            Random _random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public static string ToJsonClear(this string value)
        {
            const char BACK_SLASH = '\\';
            const char SLASH = '/';
            const char DBL_QUOTE = '"';

            var output = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                switch (c)
                {
                    case SLASH:
                        output.AppendFormat("{0}{1}", BACK_SLASH, SLASH);
                        break;

                    case BACK_SLASH:
                        output.AppendFormat("{0}{0}", BACK_SLASH);
                        break;

                    case DBL_QUOTE:
                        output.AppendFormat("{0}{1}", BACK_SLASH, DBL_QUOTE);
                        break;

                    default:
                        output.Append(c);
                        break;
                }
            }

            return output.ToString();
        }

        public static string onGetTimeElapse(this DateTime date)
        {
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan();
            time = now.Subtract(date);

            if (time.Days > 0)
            {
                return string.Format("hace {0} dias", time.Days);
            }

            if (time.Hours > 0)
            {
                return string.Format("hace {0} horas", time.Hours);
            }

            if (time.Minutes > 0)
            {
                return string.Format("hace {0} minutos", time.Minutes);
            }

            return string.Format("hace un momento");
        }

        public static DateTime ToPacific(this DateTime datetime)
        {
            try
            {
                TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                return TimeZoneInfo.ConvertTime(datetime, zone);
            }
            catch
            {
                return datetime;
            }
        }

        public static string TimeAgo(this DateTime dateTime, int time_zone = 0)
        {
            string result = string.Empty;
            var timeSpan = DateTime.UtcNow.AddHours(time_zone).Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("hace {0} segundos", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("hace {0} minutos", timeSpan.Minutes) :
                    "hace un minuto";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("hace {0} horas", timeSpan.Hours) :
                    "hace una hora";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("hace {0} dias", timeSpan.Days) :
                    "ayer";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("hace {0} meses", timeSpan.Days / 30) :
                    "hace un mes";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("hace {0} años", timeSpan.Days / 365) :
                    "hace un año";
            }

            return result;
        }
        public static string FirstCharToUpper(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
                return cultInfo.ToTitleCase(input);
            }

            return input;
        }

    }
}
