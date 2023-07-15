using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Plugin.UI.Xaml.Calendar
{
    public interface ICalendarBehavior
    {
        Command<IEvent> SelectedEvent { get; set; }
        Command<DateTime> SelectedDate { get; set; }
        ObservableCollection<IEvent> Events { get; set; }
    }
}