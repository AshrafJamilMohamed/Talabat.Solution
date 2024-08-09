using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExtention
    {
        public static async Task<ApplicationUser?> FindUserWithAddressAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            var Email = user.FindFirstValue(ClaimTypes.Email);
            var User = await userManager.Users.Include(E => E.Address)
                                              .FirstOrDefaultAsync(u => u.NormalizedEmail == Email.ToUpper());
            return User;

        }
    }
}
