using EmergencyTask.API.Enum;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmergencyTask.Model
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

        public static async void SetUserLogin(Usuario user)
        {
            if (user == null) return;
            DataBase db = new DataBase();
            db.Usuario.Clear();
            db.Usuario.Add(user);
            await db.Usuario.SaveChanges();
        }

        public static Usuario GetUserLogin(Action<Usuario> success = null, Action fail = null)
        {
            DataBase db = new DataBase();
            var usuario = db.Usuario.LastOrDefault();
            if (usuario != null) success?.Invoke(usuario);
            else fail?.Invoke();
            return usuario;
        }

        public static async void Delete()
        {
            DataBase db = new DataBase();
            db.Usuario.Clear();
            await db.Usuario.SaveChanges();
        }

        public async Task SaveChanges()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            var newuser = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(json);
            DataBase db = new DataBase();
            db.Usuario.Clear();
            db.Usuario.Add(newuser);
            await db.Usuario.SaveChanges();
        }
    }
}
