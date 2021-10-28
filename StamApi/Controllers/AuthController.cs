using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApplicationServices.Helpers;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthHelper _authHelper;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(IIdentityRepository identityRepository, UserManager<IdentityUser> userManager,
            IAuthHelper authHelper, SignInManager<IdentityUser> signInManager)
        {
            _identityRepository = identityRepository;
            _userManager = userManager;
            _authHelper = authHelper;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {

            IdentityUser user = await this._userManager.FindByEmailAsync(login.Email);

            if (user != null)
            {
                var he = (await this._signInManager.PasswordSignInAsync(user, login.Password, true,
                    true));
                if (he.Succeeded)
                {
                    return Ok(new {Token = await _authHelper.GenerateToken(login.Email)});
                }
                else
                {
                    return BadRequest();
                }
            }

            return BadRequest();

        }
    }
}