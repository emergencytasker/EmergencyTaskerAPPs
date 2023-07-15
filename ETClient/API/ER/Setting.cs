using Plugin.Net.Http;
using System;

namespace ETClient.API.ER
{
    public class Setting : IEntityBase
    {
        public int id { get; set; }
        public string opcion { get; set; }
        public string valor { get; set; }
        public string tipo { get; set; }
        public int idperfil { get; set; }
        public int eliminado { get; set; }

        public T As<T>()
        {
            try
            {
                return (T)Convert.ChangeType(valor, typeof(T));
            }
            catch { }
            return default(T);
        }
    }
}