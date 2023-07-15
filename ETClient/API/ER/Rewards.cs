using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Rewards : IEntityBase
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int variable { get; set; }
        public double valor { get; set; }
        public int requierereferido { get; set; }
        public int idperfil { get; set; }
        public int eliminado { get; set; }
        public int requierereferencia { get; set; }
        public int trabajoterminado { get; set; }
        public string descripcion { get; set; }
    }
}
