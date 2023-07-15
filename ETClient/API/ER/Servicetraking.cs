using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Servicetraking : IEntityBase
    {

        public int id { get; set; }
        public int idestadoservicio { get; set; }
        public int idsolicitudservicio { get; set; }
        public string fecha { get; set; }
        public int eliminado { get; set; }
        public int idusuario { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public int tieneubicacion { get; set; }
    }
}