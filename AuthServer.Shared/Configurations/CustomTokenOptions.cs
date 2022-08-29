using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Shared.Configurations
{
    public class CustomTokenOptions
    {
        public List<string> Audience { get; set; } //Token'ı alacak adres
        public string Issuer { get; set; } //Token'ı gönderecek adres
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }
}
