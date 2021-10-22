using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Dtos.Models;

namespace WebApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IService<Patient> _patientService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public PatientController(IWebHostEnvironment webHostEnvironment, IService<Patient> patientService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._webHostEnvironment = webHostEnvironment;
            _patientService = patientService;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }


        // // GET: Patient/Details/5
        // public ActionResult Details(int id)
        // {
        //     return View();
        // }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // GET: Patient/Create
        public ActionResult CreatePartial()
        {
            return PartialView("_Create");
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PatientDto registerDto, IFormFile picture)
        {
            if
            (
                ModelState.IsValid
                && (await this._userManager.FindByNameAsync(registerDto.Email) == null)
                && !(this._patientService.Get().Any(p => p.Email.Equals(registerDto.Email)))
            )
            {
                int patientNumber = _patientService.Get().Count();
                string pictureUrl = ProcessUploadedFile(picture);
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
                return RedirectToAction("Create","Dossier");
            }
            ViewBag.error = "Something went wrong, please try again later";
            return PartialView("_Create", registerDto);
        }

        // // GET: Patient/Edit/5
        // public ActionResult Edit(int id)
        // {
        //     return View();
        // }

        // POST: Patient/Edit/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Edit(int id, PatientDto patientDto)
        // {
        //     try
        //     {
        //         // TODO: Add update logic here
        //
        //         return RedirectToAction();
        //     }
        //     catch
        //     {
        //         return View();
        //     }
        // }


        // POST: Patient/Delete/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Delete(int id, PatientDto patientDto)
        // {
        //     try
        //     {
        //         // TODO: Add delete logic here
        //
        //         return RedirectToAction();
        //     }
        //     catch
        //     {
        //         return View("Details");
        //     }
        // }

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