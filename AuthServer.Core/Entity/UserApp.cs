using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Entity
{
    public class UserApp:IdentityUser
    {
        //IdentityUser API'sinden gelen default degerler de vardir.
        public string City { get; set; }

    }
}
