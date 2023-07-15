using Plugin.Net.Http;
using System.Threading.Tasks;

namespace ETClient.API.ER
{
    public class Service : IEntityBase
    {
        public Service()
        {

        }

        public Service(Requestservice currentService)
        {
            id = currentService.idservicio;
            idusuario = currentService.trabajador;
            costo = currentService.costoporhora;
            idcategoria = currentService.idcategoria;
            idsubcategoria = currentService.idsubcategoria;
            eliminado = 0;
        }

        public int id { get; set; }
        public int idusuario { get; set; }
        public int idcategoria { get; set; }
        public int idsubcategoria { get; set; }
        public double costo { get; set; }
        public int eliminado { get; set; }

        public async Task<Subcategory> GetSubcategory()
        {
            var item = await Client.Subcategory.Get(idsubcategoria);
            return item != null && item.id > 0 ? item : null;
        }

        public async Task<User> GetUser()
        {
            var item = await Client.User.Get(idusuario);
            return item != null && item.id > 0 ? item : null;
        }
    }
}