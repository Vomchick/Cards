using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CardsAPI.Auth.Common
{
    public class AuthOptions
    {
        public string Issuer { get; set; } //Кто сгенерировал токен
        public string Audience { get; set; } //Кому предназначался
        public string Secret { get; set; }
        public int TokenLifetime { get; set; } // secs
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}