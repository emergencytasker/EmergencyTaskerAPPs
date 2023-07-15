namespace ETClient.Models
{
    public class ChatNotification
    {
        public DateTime Date { set; get; }
        public int UserID { set;get;}
        public string Channel { set; get; }
        public string Message { set; get; }
    }
}
