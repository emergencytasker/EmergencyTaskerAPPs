using System;
using System.Collections.Generic;
using RestSharp;

namespace Plugin.Net.Http
{
    public interface IRestResponse<T>
    {
        string Content { get; set; }
        Exception Exception { get; set; }
        bool HasExecute { get; set; }
        Method Method { get; set; }
        IEnumerable<Parameter> Parameters { get; set; }
        string Path { get; set; }
        T Result { get; set; }
        string ToString();
    }
}