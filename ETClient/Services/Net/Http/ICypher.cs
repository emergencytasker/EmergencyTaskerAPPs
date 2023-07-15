
using System.Collections.Generic;

namespace Plugin.Net.Http
{
    public interface ICypher
    {

        void Options(Dictionary<string, object> options);
        string Decrypt(string crypt);

    }
}
