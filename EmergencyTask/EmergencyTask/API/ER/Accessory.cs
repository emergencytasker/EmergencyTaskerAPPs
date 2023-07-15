using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class Accessory : IEntityBase
    {
        public int id { get; set; }
        public int eliminado { get; set; }
        public string fecha { get; set; }
        public int cantidad { get; set; }
        public string nombre { get; set; }
        public double? costo { get; set; }
        public int idsolicitudservicio { get; set; }
        public int? idticket { get; set; }
    }
}