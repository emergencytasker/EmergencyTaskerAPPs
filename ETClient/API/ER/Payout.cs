using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Payout : IEntityBase
    {
        public int id { get; set; }
        public string routingnumber { get; set; }
        public string accountnumber { get; set; }
        public double cantidad { get; set; }
        public int idsaldo { get; set; }
        public int eliminado { get; set; }
    }
}