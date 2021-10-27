﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Auth;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IService<Patient> _patientService;

        public AccountController(IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager,
            IRepository<Doctor> doctorRepository, IRepository<Student> studentRepository,
            IService<Patient> patientService)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
            _doctorRepository = doctorRepository;
            _studentRepository = studentRepository;
            _patientService = patientService;
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();

            return this.RedirectToAction("Register", "Account");
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
                    BigNumber = registerDto.BigNumber
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

            
            if (ModelState.ErrorCount > 0){
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
                    StudentNumber = registerDto.StudentNumber
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
        public async Task<IActionResult> PostRegisterPatient(PatientRegisterDto registerDto)
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
                if(!(this._patientService.Get().Any(p => p.Email.Equals(registerDto.Email))))
                {
                    int patientNumber = _patientService.Get().Count();
                    string pictureUrl = ProcessUploadedFile(registerDto.Picture);
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
                        PatientNumber = patientNumber.ToString(),
                        IdNumber = registerDto.IdNumber
                    });
                }
                IdentityUser user = new IdentityUser(registerDto.Email) {Email = registerDto.Email};
                await this._userManager.CreateAsync(user, registerDto.Password);
                
                if (! await _roleManager.RoleExistsAsync("Patient"))
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
                    var he = (await this._signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, true));
                    if (he.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
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
        
        private string ProcessUploadedFile(IFormFile picture)
        {
            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            uniqueFileName = Guid.NewGuid().ToString() + "_" + picture.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                picture.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}