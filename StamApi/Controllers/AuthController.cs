using System.Threading.Tasks;
using ApplicationServices.Helpers;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Logs you in
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/login
        /// {
        ///     "email": "Drik@deDoktor.com",
        ///     "password": "GxEZMx8QUJTn8Z3"
        ///}
        ///
        /// </remarks>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>A newly generated jwt token</returns>
        /// <response code="200">Returns the newly created token</response>
        /// <response code="400">If the credentials are wrong</response>    
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