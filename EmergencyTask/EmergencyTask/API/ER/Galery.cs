using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class Galery : IEntityBase
    {

        public int id { get; set; }
        public int eliminado { get; set; }
        public string path { get; set; }
        public string descripcion { get; set; }
        public int idusuario { get; set; }

    }
}
