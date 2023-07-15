using Plugin.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Net.Http
{
    /*
    public static class ApiExtensions
    {

        /// <summary>
        /// Busca en la api a traves de una lista de ids
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> Get<T>(this IApi<T> api, IEnumerable<int> ids) where T : IEntityBase, new()
        {
            var usuarios = await api.Where(u => u.id).In(ids).Execute();
            if (!usuarios.HasExecute) return new List<T>();
            var result = (usuarios.Result ?? new List<T>()).Where(u => u.eliminado == 0);
            return result;
        }

        /// <summary>
        /// Obtiene una lista con un Query a traves del objeto a devolver basada en la logica del negocio
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> Where<T>(this IApi<T> api, T element) where T : IEntityBase, new()
        {
            var response = await api.Query(element) ?? new List<T>();
            return response.Where(r => r.eliminado == 0);
        }

        /// <summary>
        /// Obtiene una lista de todos los elementos, basada en la logica de negocio
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> Get<T>(this IApi<T> api) where T : IEntityBase, new()
        {
            var response = await api.All() ?? new List<T>();
            return response.Where(r => r.eliminado == 0);
        }

    }
    */
}