using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class AppCenter : IEntityBase
    {
        public int id { get; set; }
        public string platform { get; set; }
        public string appcenterid { get; set; }
        public int idusuario { get; set; }
        public string fecha { get; set; }
        public int eliminado { get; set; }
    }
}