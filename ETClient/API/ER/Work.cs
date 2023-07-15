using Plugin.Net.Http;
using System;

namespace ETClient.API.ER
{
    public class Work : IEntityBase
    {
        public int id { get; set; }
        public string puesto { get; set; }
        public string empresa { get; set; }
        public string descripcion { get; set; }
        public string inicio { get; set; }
        public string fin { get; set; }
        public int idusuario { get; set; }
        public int eliminado { get; set; }
    }
}