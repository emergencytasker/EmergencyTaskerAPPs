using System;
using System.Collections.Generic;

namespace EmergencyTask.Model
{
    public class Variable
    {

        public bool Introduction { get; set; }
        public string AppCenterId { get; set; }
        public bool CanReceivedNotifications { get; set; }
        public List<string> LastNumbers { get; set; }
        public DateTime? StartWaitConfirmation { get; set; }
    }
}