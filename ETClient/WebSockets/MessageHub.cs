using Microsoft.AspNetCore.SignalR;

namespace ETClient.WebSockets
{
	public class MessageHub : Hub
	{
		public override Task OnConnectedAsync()
		{
			Console.WriteLine($"[OnConnectedAsync] {this.Context.ConnectionId}");
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			Console.WriteLine($"[OnDisconnectedAsync] {this.Context.ConnectionId} => {exception.Message}");
			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}
	}
}
