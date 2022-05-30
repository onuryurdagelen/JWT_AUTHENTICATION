using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.DTO
{
    public class ClientTokenDto
    {
        public string AccessToken { get; set; }

        public DateTime AccessTokenExpirationDate { get; set; }
    }
}
