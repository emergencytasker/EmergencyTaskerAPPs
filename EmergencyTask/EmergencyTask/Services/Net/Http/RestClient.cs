using EmergencyTask.API;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Net.Http
{
    public class RestClient : IRestClient
    {
        public string BaseUrl { get; set; }
        public Dictionary<string, string> Headers { get; }
        public ICypher Cypher { get; set; }
        public ILogger Logger { get; set; }

        public RestClient(string baseurl, Dictionary<string, string> headers)
        {
            BaseUrl = baseurl;
            Headers = headers;
        }

        public async Task<IRestResponse<T>> ExecuteTaskAsync<T>(string endpoint, Method method, List<Parameter> parameters = null, List<File> files = null, object body = null)
        {
            var url = $"{BaseUrl}{endpoint}";
            var Client = new RestSharp.RestClient(url);
            var Request = new RestRequest(method);

            if (Headers != null)
            {
                foreach (var header in Headers)
                {
                    Request.AddHeader(header.Key, header.Value);
                }
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    Request.AddParameter(parameter);
                }
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    Request.AddFile(file.Name, file.Bytes, file.FileName);
                }
            }

            if (body != null)
            {
                Request.AddParameter("json", JsonConvert.SerializeObject(body), "application/json", ParameterType.RequestBody);
            }

            var restresponse = new RestResponse<T>
            {
                Path = url,
                Method = method,
                Parameters = parameters
            };

            if (Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
            {
                try
                {
                    restresponse.HasExecute = true;
                    var response = await Client.ExecuteTaskAsync(Request);
                    if (response != null)
                    {
                        var responsecontent = response.Content;
                        restresponse.Content = responsecontent;

                        if(typeof(T) == typeof(string))
                        {
                            restresponse.Result = (T) Convert.ChangeType(responsecontent, typeof(T));
                        }
                        else
                        {
                            if (Cypher != null)
                            {
                                var json = Cypher.Decrypt(responsecontent);
                                restresponse.Result = JsonConvert.DeserializeObject<T>(json);
                            }
                            else
                            {
                                restresponse.Result = JsonConvert.DeserializeObject<T>(responsecontent);
                            }
                        }
                    }
                    else
                    {
                        restresponse.Result = default(T);
                    }
                }
                catch (Exception ex)
                {
                    restresponse.HasExecute = false;
                    restresponse.Result = default(T);
                    restresponse.Exception = ex;
                }
            }
            else
            {
                restresponse.HasExecute = false;
            }

            System.Diagnostics.Debug.WriteLine(restresponse);
            if(Logger == null)
            {
                Logger = new FileLogger();
            }
            await Logger.Write(restresponse.ToString());
            return restresponse;
        }

        /// <summary>
        /// Keys are lowwer
        /// </summary>
        /// <param name="icypher">Cypher</param>
        /// <param name="options">Options of Custom Cypher</param>
        public void SetCypher(ICypher icypher, Dictionary<string, object> options = null)
        {
            if (icypher == null) return;
            if (options != null)
            {
                icypher.Options(options);
            }
            Cypher = icypher;
        }

        public void SetLogger(ILogger logger)
        {
            Logger = logger;
        }
    }

    public class RestResponse<T> : IRestResponse<T>
    {
        public string Path { get; set; }
        public Method Method { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; }
        public bool HasExecute { get; set; }
        public string Content { get; set; }
        public T Result { get; set; }
        public Exception Exception { get; set; }

        public override string ToString()
        {
            var str = "\r=====================\rREST OPERATION\r";
            str += $"Fecha: {DateTime.Now.ToString("yyyy-MM-dd hh:mm tt")}\r";
            str += $"[{Method}] {Path} => {HasExecute}\r";
            if (Parameters != null)
            {
                str += "PARAMETERS\r\t";
                foreach (var parameter in Parameters)
                {
                    str += $"{parameter.Name} => {parameter.Value} ";
                }
                str += "\r";
            }
            str += "RESPONSE\r";
            str += $"{Content}\r";
            if (Exception != null) str += $"{Exception.Message} {Exception.StackTrace}\r";
            str += "END REST OPERATION\r=====================\r";
            return str;
        }
    }
}