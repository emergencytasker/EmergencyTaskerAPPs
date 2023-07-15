using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Language : IEntityBase
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int eliminado { get; set; }
    }
}