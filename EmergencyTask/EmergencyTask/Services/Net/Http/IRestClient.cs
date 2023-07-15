using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace Plugin.Net.Http
{
    public interface IRestClient
    {
        string BaseUrl { get; set; }
        Dictionary<string, string> Headers { get; }
        ICypher Cypher { get; set; }
        Task<IRestResponse<T>> ExecuteTaskAsync<T>(string endpoint, Method method, List<Parameter> parameters = null, List<File> files = null, object body = null);
        void SetCypher(ICypher icypher, Dictionary<string, object> options = null);
        void SetLogger(ILogger logger);
    }
}