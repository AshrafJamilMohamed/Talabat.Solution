using Microsoft.AspNetCore.Identity;

namespace Talabat.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DispalyName { get; set; } = string.Empty;
        public Address? Address { get; set; }
    }
}
