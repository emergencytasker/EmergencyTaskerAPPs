using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.Services
{
	public class StartLongRunningTaskMessage { }
	public class StopLongRunningTaskMessage { }
	public class TickedMessage
	{
		public int Seconds { get; set; }
	}

	public class CancelledMessage
	{

	}

	public class TaskCounter { 
		public async Task RunCounter(CancellationToken token)
		{
			await Task.Run(async () => {
				while (true)
				{
					token.ThrowIfCancellationRequested();
					await Task.Delay(1000);
					var message = new TickedMessage
					{
						Seconds = 1
					};
					Device.BeginInvokeOnMainThread(() => {
						MessagingCenter.Instance.Send(message, "TickedMessage");
					});
				}
			}, token);
		}
	}

	
}