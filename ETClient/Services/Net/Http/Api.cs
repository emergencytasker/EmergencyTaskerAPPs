using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using RestSharp;

namespace Plugin.Net.Http
{
    public class Api<T> : IApi<T> where T : IEntityBase, new()
    {

        public Dictionary<string, string> Headers { get; }
        public string Name { get; }
        public IQueryBuilder<T> Builder { get; set; }
        public IRestClient RestClient { get; set; }

        public Api(IRestClient client)
        {
            RestClient = client;
            Name = typeof(T).Name;
            Builder = new QueryBuilder<T>(RestClient, $"v1/{Name}");
        }

        /// <summary>
        /// Devuelve todos los elementos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Get()
        {
            var response = await All() ?? new List<T>();
            return Enumerable.Where(response, r => r.eliminado == 0);
        }

        /// <summary>
        /// Obtiene un elemento especificado por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> Get(int id)
        {
            var result = await RestClient.ExecuteTaskAsync<T>($"v1/{Name}/{id}", Method.GET);
            if (!result.HasExecute) return default(T);
            return result.Result;
        }

        /// <summary>
        /// Obtiene los elementos filtrados por ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Get(IEnumerable<object> ids)
        {
            var usuarios = await Where(u => u.id).In(ids).Execute();
            if (!usuarios.HasExecute) return new List<T>();
            var result = Enumerable.Where(usuarios.Result ?? new List<T>(), u => u.eliminado == 0);
            return result;
        }

        /// <summary>
        /// Obtiene un elemento especificado por id y una lista de propiedades especificadas por expresion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public virtual async Task<T> Get(int id, params Expression<Func<T, object>>[] expressions)
        {
            var properties = StaticReflection.GetMemberNames(expressions);
            var result = await RestClient.ExecuteTaskAsync<T>($"v1/{Name}/{id}", Method.GET, new List<Parameter>
            {
                new Parameter("props", properties, ParameterType.GetOrPost)
            });
            if (!result.HasExecute) return default(T);
            return result.Result;
        }

        /// <summary>
        /// Obtiene todos los elementos
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> All()
        {
            var result = await RestClient.ExecuteTaskAsync<List<T>>($"v1/{Name}", Method.GET);
            if (!result.HasExecute) return new List<T>();
            return result.Result;
        }

        /// <summary>
        /// Agrega un elemento
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual async Task<T> Add(T element)
        {
            var parameters = new List<Parameter>();
            var propertiesinfo = element.GetType().GetProperties();
            foreach (PropertyInfo propertyinfo in propertiesinfo)
            {
                var key = propertyinfo.Name;
                var value = propertyinfo.GetValue(element, null);
                if (!IsNullOrDefault(value))
                {
                    parameters.Add(new Parameter(key, value != null ? value.ToString() : "", ParameterType.GetOrPost));
                }
            }

            var result = await RestClient.ExecuteTaskAsync<T>($"v1/{Name}", Method.POST, parameters);
            if (!result.HasExecute) return default(T);
            return result.Result;
        }

        /// <summary>
        /// Agrega una lista de elementos
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Add(IEnumerable<T> elements)
        {
            var result = await RestClient.ExecuteTaskAsync<List<T>>($"v1/{Name}", Method.POST, body: elements);
            if (!result.HasExecute) return default(List<T>);
            return result.Result;
        }

        /// <summary>
        /// Actualiza un elemento a traves del id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual async Task<T> Update(int id, IDictionary<string, string> parameters)
        {
            var putdata = new List<Parameter>();
            foreach (var parameter in parameters)
            {
                putdata.Add(new Parameter(parameter.Key, parameter.Value, ParameterType.GetOrPost));
            }
            var result = await RestClient.ExecuteTaskAsync<T>($"v1/{Name}/{id}", Method.PATCH, putdata);
            if (!result.HasExecute) return default(T);
            return result.Result;
        }

        /// <summary>
        /// Elimina un objeto a traves del ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<DeleteResponse> Delete(int id)
        {
            var result = await RestClient.ExecuteTaskAsync<DeleteResponse>($"v1/{Name}/{id}", Method.DELETE);
            if (!result.HasExecute) return new DeleteResponse { id = id, delete = 0 };
            return result.Result;
        }

        /// <summary>
        /// Realiza una peticion REST
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual async Task<K> ExecuteAsync<K>(string endpoint, Method method, IEnumerable<Parameter> parameters = null)
        {
            var result = await RestClient.ExecuteTaskAsync<K>($"{Name}/{endpoint}", method, parameters.ToList());
            if (!result.HasExecute) return default(K);
            return result.Result;
        }

        /// <summary>
        /// Busca elementos a traves de un query basado en el objeto de respuesta
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Query(T where)
        {
            var parameters = new List<Parameter>();
            var propertiesinfo = where.GetType().GetProperties();
            foreach (PropertyInfo propertyinfo in propertiesinfo)
            {
                var key = propertyinfo.Name;
                var value = propertyinfo.GetValue(where, null);

                if (!IsNullOrDefault(value))
                {
                    parameters.Add(new Parameter(key, value, ParameterType.GetOrPost));
                }
            }
            var result = await RestClient.ExecuteTaskAsync<List<T>>($"v1/{Name}", Method.GET, parameters);
            if (!result.HasExecute) return new List<T>();
            return result.Result;
        }

        /// <summary>
        /// Obtiene una lista basada en un Query a traves del objeto a devolver
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Where(T element)
        {
            var response = await Query(element) ?? new List<T>();
            return Enumerable.Where(response, r => r.eliminado == 0);
        }

        /// <summary>
        /// Realiza un WHERE basado en una funcion de expresion, requiere combinacion de IN, Execute
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public virtual IQueryBuilder<T> Where(params Expression<Func<T, object>>[] function)
        {
            return Builder.Where(function);
        }

        private bool IsNullOrDefault<X>(X argument)
        {
            if (argument == null) return true;
            if (Equals(argument, default(X))) return true;
            Type methodType = typeof(X);
            if (Nullable.GetUnderlyingType(methodType) != null) return false;
            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }
            return false;
        }
    }

    public class QueryBuilder<T> : IQueryBuilder<T>
    {
        private string EndPoint { get; set; }
        private List<Parameter> Parameters { get; set; }
        public IRestClient RestClient { get; set; }
        public QueryBuilder(IRestClient client, string end_point)
        {
            RestClient = client;
            EndPoint = end_point;
        }

        private Expression<Func<T, object>>[] CurrentExpression { get; set; }

        public IQueryBuilder<T> Where(params Expression<Func<T, object>>[] expression)
        {
            Parameters = null;
            CurrentExpression = expression;
            return this;
        }

        public IQueryBuilder<T> In(params IEnumerable<object>[] comparers)
        {
            if (CurrentExpression == null) throw new InvalidOperationException("Before Call Where Function");
            if (comparers == null) return null;
            var membernames = StaticReflection.GetMemberNames(CurrentExpression);
            if (membernames != null && comparers.Length != membernames.Count)
            {
                throw new ArgumentException("Size of Membernames & Comparers is distinct");
            }
            Parameters = new List<Parameter>();
            for (int i = 0; i < membernames.Count; i++)
            {
                var comparer = comparers[i];
                var values = string.Join("|", comparer);
                var membername = membernames[i];
                Parameters.Add(new Parameter(membername, values, ParameterType.GetOrPost));
            }
            return this;
        }

        public async Task<IRestResponse<IEnumerable<T>>> Execute()
        {
            if (Parameters == null) throw new InvalidOperationException("Before Call Where In");
            return await RestClient.ExecuteTaskAsync<IEnumerable<T>>(EndPoint, Method.GET, Parameters);
        }
    }
}