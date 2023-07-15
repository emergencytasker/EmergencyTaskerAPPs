using System;
using Xamarin.Forms;

namespace Plugin.UI.Xaml.Calendar
{
    public class Event : IEvent
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Color Color { get; set; } = Color.Black;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public Color TitleColor { get; set; } = Color.Black;
        public Color DetailColor { get; set; } = Color.Black;
        public string StartHour
        {
            get { return StartDate.ToShortTimeString(); }
        }
        public string EndHour
        {
            get { return EndDate.AddHours(1).ToShortTimeString(); }
        }

        public string Status{ get; set;}
    }
}