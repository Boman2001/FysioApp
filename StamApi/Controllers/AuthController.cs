using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StamApi.Models.Auth;

namespace StamApi.Controllers
{

    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IRepository<User> _userInformationRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(
            IIdentityRepository identityRepository,
            IRepository<User> userInformationRepository,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _identityRepository = identityRepository;
            _userInformationRepository = userInformationRepository;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _identityRepository.GetUserByEmail(login.Email);
            var userInformation = _userInformationRepository.Get(u => u.Id.ToString() == user.Id).FirstOrDefault();

            if (user == null || userInformation == null) return NotFound();
            try
            {
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("Email", user.Email) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("frambozenvlavoetbal")), SecurityAlgorithms.HmacSha256Signature)
                };
                SecurityToken token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

                return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(token)});
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

    }
}