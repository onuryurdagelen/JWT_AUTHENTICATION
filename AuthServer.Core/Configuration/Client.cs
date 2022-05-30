using System.Collections.Generic;

namespace AuthServer.Core.Configuration
{
    public class Client
    {
        public string Id { get; set; }
        public string Secret { get; set; }


        //www.myapi1.com www.mayapi2.com
        public List<string> Audiences { get; set; } // hangi API'lerin erisebilecegini bilgisi tutulur.

    }
}
