using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Log : IEntityBase
    {
        public string controller { get; set; }
        public string method { get; set; }
        public string process { get; set; }
        public string parameters { get; set; }
        public int id { get; set; }
        public int eliminado { get; set; }
        public string response { get; set; }
    }
}