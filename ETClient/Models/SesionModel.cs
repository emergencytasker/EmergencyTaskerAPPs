using ETClient.API.Enum;
using ETClient.API.ER;

namespace ETClient.Models
{
    public class SesionModel
    {
        public bool Authenticated { set; get; }
        public User User { set; get; }
        public Perfil Perfil { private set; get; }
        public string Lang { private set; get; }

        public SesionModel()
        {
            Perfil = Perfil.Client;
            Lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }
    }
}
