using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocketIOClient;

namespace Plugin.Net.Socket
{
    public class SocketIO : ISocket
    {
        public bool IsConnected { get; set; }
        public event EventHandler<bool> ConnectionStatus;
        public event EventHandler<Message> MessageReceived;
        public string BaseUrl { get; }
        private SocketIOClient.SocketIO Client { get; set; }
        public string[] Channels { get; set; }
       // public NetworkAccess Last { get; set; } = NetworkAccess.Unknown;

        public SocketIO(string host)
        {
            BaseUrl = host;
            Client = new SocketIOClient.SocketIO(BaseUrl);
           // Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        /*
        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                Last = e.NetworkAccess;
                return;
            }

            if (Last == NetworkAccess.Internet || Last == NetworkAccess.Unknown)
            {
                Last = e.NetworkAccess;
                return;
            }

            if (Client == null || Client.State == SocketIOState.Connected)
            {
                Last = e.NetworkAccess;
                return;
            }

            Last = e.NetworkAccess;
            await Connect();
        }
        */
        public async void Close()
        {
            try
            {
                await Client.CloseAsync();
            }
            catch { }
        }

        public async Task Subscribe(params string[] channels)
        {
            Channels = channels;
            await Connect();
        }

        private async Task Connect()
        {
            try
            {
                Client.OnClosed += Client_OnClosed;
                Client.OnConnected += Client_OnConnected;
                await Client.ConnectAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void Client_OnConnected()
        {
            IsConnected = true;
            ConnectionStatus?.Invoke(this, IsConnected);
            if (Channels != null && Channels.Length > 0)
            {
                foreach (var channel in Channels)
                {
                    Client.On(channel, res =>
                    {
                        System.Diagnostics.Debug.WriteLine(res.Text, channel);
                        MessageReceived?.Invoke(this, new Message
                        {
                            Text = res.Text,
                            RawText = res.Text,
                            Channel = channel
                        });
                    });
                }
            }
        }

        private void Client_OnClosed(ServerCloseReason reason)
        {
            if (reason == ServerCloseReason.ClosedByServer)
            {
                IsConnected = false;
                ConnectionStatus?.Invoke(this, IsConnected);
            }
            else if (reason == ServerCloseReason.ClosedByClient)
            {
                IsConnected = false;
            }
        }

        public async Task<List<Dictionary<string, string>>> History(string channel, int count = 100)
        {
            try
            {
                var endpoint = "/history";
                var httpclient = new RestSharp.RestClient(BaseUrl);
                var request = new RestSharp.RestRequest(endpoint, RestSharp.Method.GET);
                request.AddParameter("channel", channel, RestSharp.ParameterType.GetOrPost);
                request.AddParameter("limit", count, RestSharp.ParameterType.GetOrPost);
                var response = await httpclient.ExecuteAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = response.Content;
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return new List<Dictionary<string, string>>();
        }

        public async Task<bool> Send(string channel, Dictionary<string, string> data)
        {
            try
            {
                var httpclient = new RestSharp.RestClient(BaseUrl);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var bytes = Encoding.UTF8.GetBytes(json);
                var base64 = Convert.ToBase64String(bytes);
                var endpoint = "/push";
                var request = new RestSharp.RestRequest(endpoint, RestSharp.Method.GET);
                request.AddParameter("channel", channel, RestSharp.ParameterType.GetOrPost);
                request.AddParameter("text", base64, RestSharp.ParameterType.GetOrPost);
                var response = await httpclient.ExecuteAsync(request);
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return false;
        }
    }
}