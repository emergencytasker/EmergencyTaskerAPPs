using EmergencyTask.API.ER;
using System.Collections.Generic;

namespace EmergencyTask.API.Response
{
    public class Service
    {
        public int idcategoria { get; set; }
        public string categoria { get; set; }
        public string imagencategoria { get; set; }
        public int idsubcategoria { get; set; }
        public string subcategoria { get; set; }
        public string imagensubcategoria { get; set; }
        public int taskers { get; set; }
        public List<Schedule> horarios { get; set; }
    }
}