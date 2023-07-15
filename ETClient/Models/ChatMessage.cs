namespace ETClient.Models
{
    public class ChatMessage
    {
        public DateTime Date { get; internal set; }
        public string Channel { get; internal set; }
        public string Image { get; internal set; }
        public int UserId { get; internal set; }
        public string Name { get; internal set; }
        public string Message { get; internal set; }
        public int Cliente { get; internal set; }
        public int Trabajador { get; internal set; }
        public UserType UserType { get; internal set; }
    }
}
