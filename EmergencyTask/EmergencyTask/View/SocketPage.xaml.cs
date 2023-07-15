using Plugin.Net.Socket;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocketPage : ContentPage
    {
        public ISocket Socket { get; private set; }

        public SocketPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Socket = await SocketFactory.Instance.Resolve();
            if (Socket == null) return;
            Socket.MessageReceived += Socket_MessageReceived;
            Socket.Subscribe("gps");
        }

        private void Socket_MessageReceived(object sender, Message e)
        {
            System.Diagnostics.Debug.WriteLine(e.Text);
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            Socket?.Send("gps", new System.Collections.Generic.Dictionary<string, string>
            {
                { "latitud", "0" }
            });
        }
    }
}