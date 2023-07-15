using System.Threading.Tasks;

namespace Plugin.Net.Http
{
    public interface ILogger
    {

        Task Write(string log);

    }
}