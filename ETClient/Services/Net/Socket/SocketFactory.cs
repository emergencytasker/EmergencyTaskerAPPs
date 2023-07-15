using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Net.Socket
{
    public class SocketFactory
    {
        private IList<Func<Task<ISocket>>> References { get; set; }

        private static SocketFactory instance;
        public static SocketFactory Instance
        {
            get
            {
                if (instance == null) instance = new SocketFactory();
                return instance;
            }
        }

        public void Register(Func<Task<ISocket>> func)
        {
            if (References == null) References = new List<Func<Task<ISocket>>>();
            References.Add(func);
        }

        public async Task<ISocket> Resolve()
        {
            if (References == null) return null;
            var implementation = References.FirstOrDefault(r => r != null);
            if (implementation == null) return null;
            return await implementation.Invoke();
        }

    }
}