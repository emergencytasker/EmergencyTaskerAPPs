using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class Balance : IEntityBase
    {
        public int id { get; set; }
        public int eliminado { get; set; }
        public int idusuario { get; set; }
        public int idperfil { get; set; }
        public int? idhito { get; set; }
        public double cantidad { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public int essolicitud { get; set; }
    }
}