using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class Reason : IEntityBase
    {
        public int id { get; set; }
        public int idrazon { get; set; }
        public string traduccion { get; set; }
        public int idlenguaje { get; set; }
        public int eliminado { get; set; }
    }
}