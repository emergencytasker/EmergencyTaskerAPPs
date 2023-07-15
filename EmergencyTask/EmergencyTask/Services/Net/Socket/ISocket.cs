using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Net.Socket
{
    public interface ISocket
    {
        bool IsConnected { get; set; }
        event EventHandler<bool> ConnectionStatus;
        event EventHandler<Message> MessageReceived;
        Task Subscribe(params string[] channels);
        void Close();
        Task<List<Dictionary<string, string>>> History(string channel, int count = 100);
        Task<List<Dictionary<string, string>>> Online(string user_id, int count = 1);
        Task<bool> Send(string channel, Dictionary<string, string> data);
    }
}