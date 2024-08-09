using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity.IdentityDataSeed
{
    public static class IdentityDbContextDataSeed
    {
        public static async Task DataSeedAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new ApplicationUser()
                {
                    DispalyName = "Ashraf Jamil",
                    UserName = "Ashraf.Jamil",
                    Email = "Ashraf.Jamil@Gmail.com"

                };

                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
        }
    }
}
