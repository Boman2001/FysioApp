using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Dtos.Auth;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IRepository<Doctor> _DoctorRepository;
        private readonly IRepository<Student> _StudentRepository;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IRepository<Doctor> doctorRepository, IRepository<Student> studentRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _DoctorRepository = doctorRepository;
            _StudentRepository = studentRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectToActionPermanent("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            Console.WriteLine(ModelState);
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostLogout()
        {
            await this.signInManager.SignOutAsync();

            return this.RedirectToAction("Register", "Auth");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostRegisterDoctor(DoctorRegisterDto registerDto)
        {
            if
            (
                ModelState.IsValid
                && (await this.userManager.FindByNameAsync(registerDto.Email) == null)
            )
            {
                IdentityUser user = new IdentityUser(registerDto.Email) {Email = registerDto.Email};
                await this.userManager.CreateAsync(user, registerDto.Password);

                await this._DoctorRepository.Add(new Doctor()
                {
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    EmployeeNumber = registerDto.EmployeeNumber,
                    BigNumber = registerDto.BigNumber
                });
                await this.userManager.AddClaimAsync(user, new Claim("Doctor", "true"));
                return await this.PostLogin(new LoginDto()
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password
                });
            }


            ViewBag.error = "Something went wrong, please try again later";
            return View("Register", registerDto);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostRegisterStudent(StudentRegisterDto registerDto)
        {
            if
            (
                ModelState.IsValid
                && (await this.userManager.FindByNameAsync(registerDto.Email) == null)
            )
            {
                IdentityUser user = new IdentityUser(registerDto.Email) {Email = registerDto.Email};
                await this.userManager.CreateAsync(user, registerDto.Password);

                await this._StudentRepository.Add(new Student()
                {
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    StudentNumber = registerDto.StudentNumber
                });
                await this.userManager.AddClaimAsync(user, new Claim("Student", "true"));
                return await this.PostLogin(new LoginDto()
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password
                });
            }

            ViewBag.error = "Something went wrong, please try again later";
            return this.RedirectToAction("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostLogin(LoginDto loginViewModel)
        {
            if (this.ModelState.IsValid)
            {
                IdentityUser user = await this.userManager.FindByEmailAsync(loginViewModel.Email);

                if (user != null)
                {
                    if ((await this.signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, true))
                        .Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            this.ModelState.AddModelError("", "Invalid Credentials");
            return this.RedirectToAction("Login", loginViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Student()
        {
            return PartialView("_StudentRegister");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Doctor()
        {
            return PartialView("_DoctorRegister");
        }
    }
}