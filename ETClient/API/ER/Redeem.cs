using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Redeem : IEntityBase
    {
        public int id { get; set; }
        public int idrecompensas { get; set; }
        public int idusuario { get; set; }
        public int reclamada { get; set; }
        public int variable { get; set; }
        public int realizado { get; set; }
        public int eliminado { get; set; }
        public int liberada { get; set; }
        public string codigo { get; set; }
        public int? referencia { get; set; }
        public int trabajoterminado { get; set; }
    }
}