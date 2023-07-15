
namespace ETClient.API
{
    public class FileLogger : Plugin.Net.Http.ILogger
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
