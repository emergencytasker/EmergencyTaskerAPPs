using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class Hito : IEntityBase
    {
        public int id { get; set; }
        public double cantidad { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public int cliente { get; set; }
        public int trabajador { get; set; }
        public int idsolicitudservicio { get; set; }
        public string chargeid { get; set; }
        public int eliminado { get; set; }
        public string fechadeautorizacion { get; set; }
        public string fechadeliberacion { get; set; }
        public string fechasolicutudreembolso { get; set; }
        public string fechadereembolso { get; set; }
        public int estado { get; set; }
        public double? cantidadtrabajador { get; set; }
        public double? cantidademergencytasker { get; set; }
        public int trabajoterminado { get; set; }
        public double costofinal { get; set; }
        public int extras { get; set; }

    }
}