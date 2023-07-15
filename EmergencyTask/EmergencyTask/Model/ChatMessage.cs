using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyTask.Model
{
    public class ChatMessage
    {
        public DateTime Date { get; internal set; }
        public string Channel { get; internal set; }
        public int UserId { get; internal set; }
        public string Message { get; internal set; }
        public int Cliente { get; internal set; }
        public int Trabajador { get; internal set; }
    }
}
