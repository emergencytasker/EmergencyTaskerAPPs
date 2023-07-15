using System.Threading.Tasks;

namespace EmergencyTask.Services
{
    public interface IShare
    {
        Task Share(string url);
    }
}
