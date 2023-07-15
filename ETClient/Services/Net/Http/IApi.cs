using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RestSharp;

namespace Plugin.Net.Http
{
    public interface IApi<T> where T : IEntityBase, new()
    {
        IQueryBuilder<T> Builder { get; set; }
        Dictionary<string, string> Headers { get; }

        Task<IEnumerable<T>> Get();
        Task<T> Get(int id);
        Task<IEnumerable<T>> Get(IEnumerable<object> ids);
        Task<T> Get(int id, params Expression<Func<T, object>>[] expressions);
        Task<IEnumerable<T>> All();

        Task<T> Add(T element);
        Task<IEnumerable<T>> Add(IEnumerable<T> elements);

        Task<T> Update(int id, IDictionary<string, string> parameters);

        Task<DeleteResponse> Delete(int id);
        
        IQueryBuilder<T> Where(params Expression<Func<T, object>>[] function);
        Task<IEnumerable<T>> Where(T element);
        Task<IEnumerable<T>> Query(T where);

        Task<K> ExecuteAsync<K>(string endpoint, Method method, IEnumerable<Parameter> parameters = null);
    }
}