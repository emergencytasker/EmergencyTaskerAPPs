using Plugin.Net.Http;
using System.Threading.Tasks;

namespace EmergencyTask.API.ER
{
    public class Notification : IEntityBase
    {
        public int id { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string fecha { get; set; }
        public int notificado { get; set; }
        public int idusuario { get; set; }
        public int eliminado { get; set; }
        public int idaction { get; set; }
        public string fechadelectura { get; set; }
        public string data { get; set; }

        public async Task<User> GetUser()
        {
            var item = await Client.User.Get(idusuario);
            return item != null && item.id > 0 ? item : null;
        }
    }
}