using Microsoft.AspNetCore.Identity;

namespace Licencjat.Models
{
    public class AdminView
    {
        public List<IdentityUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}