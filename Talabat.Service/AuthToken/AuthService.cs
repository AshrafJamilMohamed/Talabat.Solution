using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Service.Contract;

namespace Talabat.Service.AuthToken
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;

        public AuthService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            // Private Claims
            var AuthClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.DispalyName),
                new Claim(ClaimTypes.Email,user.Email)

            };

            var UserRole = await userManager.GetRolesAsync(user);

            foreach (var role in UserRole)
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));

            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"]));

            var Token = new JwtSecurityToken(
                    audience: configuration["JWT:ValidAudiance"],
                    issuer: configuration["JWT:ValidIssure"],
                    expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])),
                    claims: AuthClaims,
                    signingCredentials: new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256Signature)
                );

            // Create Token

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
