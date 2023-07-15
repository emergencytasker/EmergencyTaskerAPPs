using Plugin.Net.Http;

namespace EmergencyTask.API.ER
{
    public class Time : IEntityBase
    {
        public int id { get; set; }
        public string fechainicio { get; set; }
        public string fechafin { get; set; }
        public int idsolicitudservicio { get; set; }
        public int trabajador { get; set; }
        public string tiempo { get; set; }
        public int eliminado { get; set; }
        public int finalizado { get; set; }
        public double costo { get; set; }
        public string chargeid { get; set; }
    }
}