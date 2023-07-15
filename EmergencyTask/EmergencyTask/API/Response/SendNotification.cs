namespace EmergencyTask.API.Response
{
    public class SendNotification
    {
        public string email { get; set; }
        public int estadoemail { get; set; }
        public int estadoandroid { get; set; }
        public int estadoios { get; set; }
        public string notification { get; set; }
    }
}