using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class Cancelservice : IEntityBase
    {
        public int id { get; set; }
        public int idrazon { get; set; }
        public int idsolicitudservicio { get; set; }
        public string comentario { get; set; }
        public int eliminado { get; set; }
    }
}