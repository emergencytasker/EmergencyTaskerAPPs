using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyTask.Model
{
    public class StateModel : ModelBase
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string Comentary { get; set; }

        public DateTime Date { get; set; }
    }
}
