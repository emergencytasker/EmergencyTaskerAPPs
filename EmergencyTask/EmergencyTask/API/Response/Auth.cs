namespace EmergencyTask.API.Response
{
    public class Auth
    {

        public string token { get; set; }
        public string message { get; set; }
        public int code { get; set; }
        public bool status { get; set; }
        public int userid { get; set; }
        public int sessionid { get; set; }
    }
}