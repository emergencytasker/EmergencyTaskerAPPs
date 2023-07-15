using ETClient.API.Enum;

namespace ETClient.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int activado { get; set; }
        public string telefono { get; set; }
        public int telefonoverificado { get; set; }
        public int idperfil { get; set; }
        public int eliminado { get; set; }
        public string fecha { get; set; }
        public string identificacion { get; set; }
        public string segurosocial { get; set; }
        public int identificacionvalidada { get; set; }
        public int segurosocialvalidado { get; set; }
        public string imagen { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public int tareas { get; set; }
        public double calificacion { get; set; }
        public string documentofbi { get; set; }
        public int requierefbi { get; set; }
        public int documentofbivalidado { get; set; }
        public int facebooklogin { get; set; }
        public string facebookid { get; set; }
        public string referencia { get; set; }
        public int codigocanjeado { get; set; }
        public string referido { get; set; }
        public string token { get; set; }
        public string descripcion { get; set; }
        public int online { get; set; }
        public string lenguaje { get; set; }

        public Perfil Perfil
        {
            get
            {
                return (Perfil)idperfil;
            }
        }
    }
}
