using System;
using Xamarin.Forms;

namespace Plugin.UI.Xaml.Calendar
{
    public interface IEvent
    {
        int Id { get; set; }
        string Title { get; set; }
        string Detail { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        Color Color { get; set; }
        Color TitleColor { get; set; }
        Color DetailColor { get; set; }
        string StartHour { get; }
        string EndHour { get; }
        string Status { get; set; }
    }
}
