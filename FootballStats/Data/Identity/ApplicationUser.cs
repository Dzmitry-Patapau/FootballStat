using Microsoft.AspNetCore.Identity;

namespace FootballStats.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
    }
}
