using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Calendar : IEntityBase
    {
        public int id { get; set; }
        public int dia { get; set; }
        public string inicio { get; set; }
        public string fin { get; set; }
        public int eliminado { get; set; }
        public int idusuario { get; set; }
        public int activo { get; set; }

    }
}
