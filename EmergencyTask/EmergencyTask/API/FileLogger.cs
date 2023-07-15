using Plugin.Net.Http;
using System.Threading.Tasks;

namespace EmergencyTask.API
{
    public class FileLogger : ILogger
    {

        public FileLogger()
        {
            
        }

        public Task Write(string log)
        {
            return Task.Run(() => { });
        }
    }
}