using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Stripeuser : IEntityBase
    {
        public int id { get; set; }
        public int idusuario { get; set; }
        public string customer { get; set; }
        public string modo { get; set; }
        public int eliminado { get; set; }
    }
}