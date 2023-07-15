using Plugin.Net.Http;
using System;

namespace EmergencyTask.API.ER
{
    public class Study : IEntityBase
    {
        public int id { get; set; }
        public string grado { get; set; }
        public string titulo { get; set; }
        public string institucion { get; set; }
        public int idusuario { get; set; }
        public int eliminado { get; set; }
    }
}