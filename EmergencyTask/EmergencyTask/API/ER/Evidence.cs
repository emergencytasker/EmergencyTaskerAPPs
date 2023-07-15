using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class Evidence : IEntityBase
    {
        public int id { get; set; }
        public string path { get; set; }
        public int idcalificacion { get; set; }
        public int idsolicitudservicio { get; set; }
        public int idusuario { get; set; }
        public int eliminado { get; set; }
        public string comentario { get; set; }
    }
}