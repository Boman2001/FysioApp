using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationServices.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthHelper(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateToken(string Email)
        {
            IdentityUser user = await  _userManager.FindByEmailAsync(Email);
            string mySecret = "ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM";
            SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
            
            List<Claim> userClaims = new List<Claim> {
                
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            userClaims.AddRange(
                
                (await this._userManager.GetRolesAsync(user)).Select(
                
                    userRole => new Claim(ClaimTypes.Role, userRole)
                )
            );
            JwtSecurityToken token = new JwtSecurityToken(
                
                
                expires: DateTime.UtcNow.AddDays(7),
                claims: userClaims,
                signingCredentials: new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256)
            );

            
            
            return  new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}