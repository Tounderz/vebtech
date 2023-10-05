using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace vebtech.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "DmitryGrudinsky";
        public const string AUDIENCE = "VebtechClients";
        const string KEY = "mysupersecret_secretkey!123";
        public const int LIFETIME = 30;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
