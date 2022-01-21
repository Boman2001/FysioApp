using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationServices.Helpers;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Auth;
using WebApp.helpers;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IService<Patient> _patientService;
        private readonly IUserService _userService;
        private readonly IAuthHelper _authHelper;

        public AccountController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager,
            IRepository<Doctor> doctorRepository, IRepository<Student> studentRepository,
            IService<Patient> patientService, IUserService userService, IAuthHelper authHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _doctorRepository = doctorRepository;
            _studentRepository = studentRepository;
            _patientService = patientService;
            _userService = userService;
            _authHelper = authHelper;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Edit()
        {
            User user = _userService.Get(u => u.Email == User.Identity.Name).First();

            if (user is Student)
            {
                Student student = await _studentRepository.Get(user.Id);
                return View("EditStudent", new StudentRegisterDto()
                {
                    FirstName = student.FirstName,
                    Preposition = student.Preposition,
                    start = student.start,
                    end = student.end,
                    Email = student.Email,
                    LastName = student.LastName,
                    StudentNumber = student.StudentNumber
                });
            }
            else if (user is Doctor)
            {
                Doctor doctor = await _doctorRepository.Get(user.Id);
                return View("EditDoctor", new DoctorRegisterDto()
                {
                    FirstName = doctor.FirstName,
                    Preposition = doctor.Preposition,
                    LastName = doctor.LastName,
                    start = doctor.start,
                    end = doctor.end,
                    Email = doctor.Email,
                    BigNumber = doctor.BigNumber,
                    EmployeeNumber = doctor.EmployeeNumber,
                    PhoneNumber = doctor.PhoneNumber
                });
            }

            return View();
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
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();

            return this.RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostRegisterDoctor(DoctorRegisterDto registerDto)
        {
            if
            (
                ModelState.IsValid
                && (await this._userManager.FindByNameAsync(registerDto.Email) == null)
            )
            {
                IdentityUser user = new IdentityUser(registerDto.Email) {Email = registerDto.Email};
                await this._userManager.CreateAsync(user, registerDto.Password);

                await this._doctorRepository.Add(new Doctor()
                {
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    EmployeeNumber = registerDto.EmployeeNumber,
                    BigNumber = registerDto.BigNumber,
                    start = registerDto.start,
                    end = registerDto.end,
                });
                if (!await _roleManager.RoleExistsAsync("Staff"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Staff"));
                }

                await this._userManager.AddToRoleAsync(user, "Staff");
                return await this.PostLogin(new LoginDto()
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password
                });
            }


            if (ModelState.ErrorCount > 0)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                allErrors.ForEach(e => { TempData["ErrorMessage"] += e.ErrorMessage + "   "; });
            }

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
                && (await this._userManager.FindByNameAsync(registerDto.Email) == null)
            )
            {
                IdentityUser user = new IdentityUser(registerDto.Email) {Email = registerDto.Email};
                await this._userManager.CreateAsync(user, registerDto.Password);

                await this._studentRepository.Add(new Student()
                {
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    StudentNumber = registerDto.StudentNumber,
                    start = registerDto.start,
                    end = registerDto.end
                });

                if (!await _roleManager.RoleExistsAsync("Staff"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Staff"));
                }

                await this._userManager.AddToRoleAsync(user, "Staff");
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
        public async Task<IActionResult> PostRegisterPatientSelf(PatientRegisterDto registerDto, IFormFile picture)
        {
            if
            (
                ModelState.IsValid
            )
            {
                if ((await this._userManager.FindByNameAsync(registerDto.Email) != null)
                    && (this._patientService.Get().Any(p => p.Email.Equals(registerDto.Email))))
                {
                    TempData["ErrorMessage"] = "Email al in gebruik";
                    return RedirectToAction("Create", "Dossier");
                }

                string patientNumber = Guid.NewGuid().ToString();
                string pictureUrl = ImageHelper.ProcessUploadedFile(picture);
                try
                {
                    await _patientService.Add(new Patient()
                    {
                        Email = registerDto.Email,
                        FirstName = registerDto.FirstName,
                        LastName = registerDto.LastName,
                        Preposition = registerDto.Preposition,
                        PhoneNumber = registerDto.PhoneNumber,
                        PictureUrl = pictureUrl,
                        BirthDay = registerDto.BirthDay,
                        Gender = registerDto.Gender,
                        PatientNumber = patientNumber,
                        IdNumber = registerDto.IdNumber,
                        Street = registerDto.Street,
                        HouseNumber = registerDto.HouseNumber,
                        PostalCode = registerDto.PostalCode,
                        City = registerDto.City,
                    });
                    
                    IdentityUser user = new IdentityUser(registerDto.Email) {Email = registerDto.Email};
                    await this._userManager.CreateAsync(user, registerDto.Password);

                    if (!await _roleManager.RoleExistsAsync("Patient"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Patient"));
                    }

                    await this._userManager.AddToRoleAsync(user, "Patient");
                    return await this.PostLogin(new LoginDto()
                    {
                        Email = registerDto.Email,
                        Password = registerDto.Password
                    });
                }
                catch (ValidationException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                allErrors.ForEach(e => { TempData["ErrorMessage"] += e.ErrorMessage + "   "; });
            }

            return View("Register");
        }
        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostRegisterPatient(RegisterDto registerDto)
        {
            if
            (
                ModelState.IsValid
            )
            {
                if ((await this._userManager.FindByNameAsync(registerDto.Email) != null))
                {
                    ViewBag.error = "You are already registered please try log in in";
                    return View("Register", registerDto);
                }

                if (_patientService.Get().Any(p => p.Email.Equals(registerDto.Email)))
                {
                    IdentityUser user = new IdentityUser(registerDto.Email) {Email = registerDto.Email};
                    await this._userManager.CreateAsync(user, registerDto.Password);

                    if (!await _roleManager.RoleExistsAsync("Patient"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Patient"));
                    }

                    await this._userManager.AddToRoleAsync(user, "Patient");
                    return await this.PostLogin(new LoginDto()
                    {
                        Email = registerDto.Email,
                        Password = registerDto.Password
                    });
                }

                ViewBag.error = "Something went wrong, please try again later";
                return View("Register", registerDto);
            }

            return View("Register", registerDto);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostLogin(LoginDto loginViewModel)
        {
            if (this.ModelState.IsValid)
            {
                IdentityUser user = await this._userManager.FindByEmailAsync(loginViewModel.Email);

                if (user != null)
                {
                    var signInResult = (await this._signInManager.PasswordSignInAsync(user, loginViewModel.Password, true,
                        true));
                    if (signInResult.Succeeded)
                    {
                        var token = await _authHelper.GenerateToken(loginViewModel.Email);
                        HttpContext.Session.Set("token", Encoding.ASCII.GetBytes(token));
                        return RedirectToAction("Index", "Appointment");
                    }
                }
            }

            this.ModelState.AddModelError("", "Invalid Credentials");
            return View("Login", loginViewModel);
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Patient()
        {
            return PartialView("_PatientRegister");
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PatientSelf()
        {
            return PartialView("_PatientSelfRegister");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> EditDoctor(DoctorRegisterDto registerDto)
        {
            if
            (
                ModelState.IsValid
            )
            {
                var user =  this._doctorRepository.Get(user => user.Email == registerDto.Email).First();
                await this._doctorRepository.Update(user.Id,new Doctor()
                {
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    EmployeeNumber = registerDto.EmployeeNumber,
                    BigNumber = registerDto.BigNumber,
                    start = registerDto.start,
                    end = registerDto.end,
                    Id = user.Id,
                    Preposition = user.Preposition,
                    CommentsCreated = user.CommentsCreated,
                    IntakesDone = user.IntakesDone,
                    IntakesSupervised = user.IntakesSupervised,
                    HeadPractisionerOf = user.HeadPractisionerOf,
                    TreatmentsDone = user.TreatmentsDone
                });
                TempData["SuccesMessage"] = "aanpassing succesvol";
                return RedirectToAction("Index", "Appointment");
            }


            if (ModelState.ErrorCount > 0)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                allErrors.ForEach(e => { TempData["ErrorMessage"] += e.ErrorMessage + "   "; });
            }

            return View("EditDoctor", registerDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> EditStudent(StudentRegisterDto registerDto)
        {
            if
            (
                ModelState.IsValid
            )
            {
                var user =  this._userService.Get(user => user.Email == registerDto.Email).First();
                await this._studentRepository.Update(user.Id,new Student()
                {
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    StudentNumber = registerDto.StudentNumber,
                    start = registerDto.start,
                    end = registerDto.end,
                    Id = user.Id,
                    Preposition = user.Preposition,
                    CommentsCreated = user.CommentsCreated,
                    IntakesDone = user.IntakesDone,
                    IntakesSupervised = user.IntakesSupervised,
                    HeadPractisionerOf = user.HeadPractisionerOf,
                    TreatmentsDone = user.TreatmentsDone
                });
                TempData["SuccesMessage"] = "aanpassing succesvol";
                return RedirectToAction("Index", "Appointment");
            }

            if (ModelState.ErrorCount > 0)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                allErrors.ForEach(e => { TempData["ErrorMessage"] += e.ErrorMessage + "   "; });
            }

            return View("EditStudent", registerDto);
        }
        
    }
}