using Newtonsoft.Json;

namespace ETClient.API.Response
{
    public class Tasker
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("activado")]
        public int Activado { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        [JsonProperty("telefonoverificado")]
        public int Telefonoverificado { get; set; }

        [JsonProperty("idperfil")]
        public int Idperfil { get; set; }

        [JsonProperty("eliminado")]
        public int Eliminado { get; set; }

        [JsonProperty("fecha")]
        public string Fecha { get; set; }

        [JsonProperty("segurosocial")]
        public string Segurosocial { get; set; }

        [JsonProperty("identificacionvalidada")]
        public int Identificacionvalidada { get; set; }

        [JsonProperty("segurosocialvalidado")]
        public int Segurosocialvalidado { get; set; }

        [JsonProperty("imagen")]
        public string Imagen { get; set; }

        [JsonProperty("identificacion")]
        public string Identificacion { get; set; }

        [JsonProperty("latitud")]
        public double? Latitud { get; set; }

        [JsonProperty("longitud")]
        public double? Longitud { get; set; }

        [JsonProperty("tareas")]
        public int Tareas { get; set; }

        [JsonProperty("calificacion")]
        public double Calificacion { get; set; }

        [JsonProperty("documentofbi")]
        public string Documentofbi { get; set; }

        [JsonProperty("requierefbi")]
        public int Requierefbi { get; set; }

        [JsonProperty("documentofbivalidado")]
        public int Documentofbivalidado { get; set; }

        [JsonProperty("facebooklogin")]
        public int Facebooklogin { get; set; }

        [JsonProperty("facebookid")]
        public string Facebookid { get; set; }

        [JsonProperty("enservicio")]
        public int Enservicio { get; set; }

        [JsonProperty("referencia")]
        public string Referencia { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("online")]
        public string Online { get; set; }

        [JsonProperty("lenguaje")]
        public string Lenguaje { get; set; }

        [JsonProperty("idservicio")]
        public int Idservicio { get; set; }

        [JsonProperty("mi")]
        public double DistanceMiles { get; set; }

        [JsonProperty("tareascompletas")]
        public int Tareascompletas { get; set; }

        [JsonProperty("costoporhora")]
        public double Costoporhora { get; set; }

        [JsonProperty("diasdesdeultimoservicio")]
        public double ElapsedDaysFromLastService { get; set; }

        [JsonProperty("diasdesdeultimasesion")]
        public double ElapsedDaysFromLastSession { get; set; }
    }
}