using Plugin.UI.Xaml.Calendar;
using System;
using System.Collections.Generic;

namespace Plugin.UI.Xaml.Calendar
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}