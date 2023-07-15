using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyTask.Model
{
    public class SupportModel : ModelBase
    {
        public int Id { get; set; }

        public string DetailService { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int IdStatus { get; set; }
    }
}
