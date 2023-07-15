using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Requestservice : IEntityBase
    {
        public int id { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public int tiemposolicitado { get; set; }
        public string direccion { get; set; }
        public string detalles { get; set; }
        public int tienehorario { get; set; }
        public string fechadeservicio { get; set; }
        public string descripcion { get; set; }
        public string categoria { get; set; }
        public string subcategoria { get; set; }
        public string fecha { get; set; }
        public int eliminado { get; set; }
        public int idcategoria { get; set; }
        public int idsubcategoria { get; set; }
        public int cliente { get; set; }
        public int trabajador { get; set; }
        public int idservicio { get; set; }
        public double costoporhora { get; set; }
        public int idestadoservicio { get; set; }
        public int tieneaccesorios { get; set; }
        public string fechainicio { get; set; }
        public string fechafin { get; set; }
        public double aceptadoaladistanciade { get; set; }

        public async Task<Service> GetService()
        {
            var item = await Client.Service.Get(idservicio);
            return item != null && item.id > 0 ? item : null;
        }

        public async Task<Subcategory> GetSubcategory()
        {
            var item = await Client.Subcategory.Get(idsubcategoria);
            return item != null && item.id > 0 ? item : null;
        }

        public async Task<User> GetTrabajador()
        {
            var item = await Client.User.Get(trabajador);
            return item != null && item.id > 0 ? item : null;
        }

        public async Task<User> GetCliente()
        {
            var item = await Client.User.Get(cliente);
            return item != null && item.id > 0 ? item : null;
        }
    }
}