using EmergencyTask.Helpers;
using EmergencyTask.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BubbleView : Grid
    {
        public string Msg { get; set; }
        public string Date { get; set; }
        public UserType UserType { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public ChatMessage Data { get; set; }
        public BubbleView(ChatMessage chatmessage, UserType usertype = UserType.Me, string name = "", string userimage = "")
        {
            InitializeComponent();
            Data = chatmessage;
            Msg = chatmessage.Message;
            Date = chatmessage.Date.ToPrettyDate();
            UserType = usertype;
            UserName = name;
            UserImage = userimage;
            Bubble.HorizontalOptions = usertype == UserType.Me ? LayoutOptions.End : LayoutOptions.Start;
            BubbleMessage.BackgroundColor = usertype == UserType.Me ? (Color)App.Current.Resources["Accent"] : Color.White;
            Message.TextColor = usertype == UserType.Me ? Color.White : Color.Black;
            Message.Text = chatmessage.Message;
            Fecha.Text = Date;
            // Fecha.TextColor = usertype == UserType.Me ? Color.FromHex("#dddddd") : Color.Gray;
            Name.IsVisible = !string.IsNullOrEmpty(name);
            Fecha.HorizontalOptions = usertype == UserType.Me ? LayoutOptions.End : LayoutOptions.Start;
            if (!string.IsNullOrEmpty(name)) {
                Name.Text = name;
                
                if (usertype == UserType.Another)
                {
                    // Name.TextColor = (Color) Application.Current.Resources["Accent"];
                    Name.HorizontalOptions = LayoutOptions.StartAndExpand;
                    Name.HorizontalTextAlignment = TextAlignment.Start;
                }
                else
                {
                    // Name.TextColor = Color.White;
                    Name.HorizontalOptions = LayoutOptions.EndAndExpand;
                    Name.HorizontalTextAlignment = TextAlignment.End;
                }
            }

            if (string.IsNullOrEmpty(userimage))
                userimage = "icon.png";
            else
                userimage = API.Client.GetPath(userimage);

            if (usertype == UserType.Me)
            {
                Image1.IsVisible = false;
                Image2.Source = userimage;
            }
            else
            {
                Image2.IsVisible = false;
                Image1.Source = userimage;
            }
        }

        public override bool Equals(object obj)
        {
            if(obj is BubbleView view)
            {
                return view.Msg == Msg && view.Date == Date
                    && view.UserType == UserType && view.UserName == UserName && Data == view.Data;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum UserType
    {
        Me, Another
    }
}