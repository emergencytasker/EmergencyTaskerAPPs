using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Review : IEntityBase
    {
        public int id { get; set; }
        public double calificacion { get; set; }
        public int idsolicitudservicio { get; set; }
        public int idusuario { get; set; }
        public int idperfil { get; set; }
        public int eliminado { get; set; }
        public string comentario { get; set; }

    }
}