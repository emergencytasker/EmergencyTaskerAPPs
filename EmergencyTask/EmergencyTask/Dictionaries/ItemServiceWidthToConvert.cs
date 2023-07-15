using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace EmergencyTask.Dictionaries
{
    public class ItemServiceWidthToConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double margin = 45;

            try
            {
                double _value = (double)value;
                return (_value / 2) - margin;
            }catch (Exception ex)
            {
                Debug.WriteLine($"[ItemServiceWidthToConvert] {ex}");
                return (App.Display.Info.Width / 2) -margin;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
