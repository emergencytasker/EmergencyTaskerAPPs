using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyTask.API.ER
{
    public class Categorylanguage
    {
        public int id { get; set; }
        public int idlenguaje { get; set; }
        public string traduccion { get; set; }
        public int idcategoria { get; set; }
    }
}
