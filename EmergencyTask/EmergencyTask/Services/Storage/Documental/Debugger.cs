using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Storage.Documental
{
    public class Debugger
    {

        public static bool Info { get; set; }

        public static void WriteLine(object data)
        {
            if (Info)
            {
                System.Diagnostics.Debug.WriteLine("YunoDB: {0}", data);
            }
        }
    }
}
