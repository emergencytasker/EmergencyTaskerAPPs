using Plugin.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmergencyTask.API.ER
{
    public class Category : IEntityBase
    {

        public int id { get; set; }
        public string nombre { get; set; }
        public string imagen { get; set; }
        public int eliminado { get; set; }

        public async Task<IEnumerable<Subcategory>> GetSubcategories()
        {
            var list = (await Client.Subcategory.Query(new Subcategory
            {
                idcategoria = id
            })) ?? new List<Subcategory>();
            return list.Where(l => l.eliminado == 0);
        }

        public async Task<IEnumerable<Service>> GetServices()
        {
            var list = (await Client.Service.Query(new Service
            {
                idcategoria = id
            })) ?? new List<Service>();
            return list.Where(l => l.eliminado == 0);
        }
    }
}