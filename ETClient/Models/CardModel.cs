using Org.BouncyCastle.Asn1.Mozilla;

namespace ETClient.Models
{
    public class CardModel
    {
        public string Owner { set; get; }
        public string Numbers { set; get; }
        public string Cvv { set; get; }
        public int Year { set; get; }
        public int Month { set; get; }
    }
}
