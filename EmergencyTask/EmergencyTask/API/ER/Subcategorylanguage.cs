using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyTask.API.ER
{
    public class Subcategorylanguage
    {
        public int id { get; set; }
        public int idlenguaje { get; set; }
        public string traduccion { get; set; }
        public int idsubcategoria { get; set; }
    }
}
