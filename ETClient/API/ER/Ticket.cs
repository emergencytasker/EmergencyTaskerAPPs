using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Ticket : IEntityBase
    {

        public int id { get; set; }
        public string imagen { get; set; }
        public double total { get; set; }
        public string detalle { get; set; }
        public int? idhito { get; set; }
        public int idsolicitudservicio { get; set; }
        public int eliminado { get; set; }

    }
}