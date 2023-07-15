using Plugin.Net.Http;
using System.Collections.Generic;
using System.Linq;

namespace ETClient.API.ER
{
    public class Subcategory : IEntityBase
    {

        public int id { get; set; }
        public string nombre { get; set; }
        public string imagen { get; set; }
        public int eliminado { get; set; }
        public int idcategoria { get; set; }
        public double costo { get; set; }
        public int tramitefbi { get; set; }
        public List<Schedule> horarios { get; set; }

        public IEnumerable<Schedule> GetSchedules()
        {
            return (horarios ?? new List<Schedule>()).Where(s => s.eliminado == 0);
        }
    }
}