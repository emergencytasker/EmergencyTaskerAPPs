using System.Collections.Generic;

namespace EmergencyTask.Model
{
    public class ServiceListModel : List<CartaModel>
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public ServiceListModel(string title, string subtitle, string image)
        {
            Title = title;
            Subtitle = subtitle;
            Image = image;
        }
    }
}