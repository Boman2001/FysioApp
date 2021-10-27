using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IService<Patient> _patientService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public PatientController(IWebHostEnvironment webHostEnvironment, IService<Patient> patientService,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._webHostEnvironment = webHostEnvironment;
            _patientService = patientService;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            IEnumerable<Patient> patients = _patientService.Get();
            ICollection<PatientDto> dtos = new List<PatientDto>();
            patients.ForEach(p => { dtos.Add(this.ToDto(p)); });

            return View(dtos);
        }

        // // GET: Patient/Details/5
        // public ActionResult Details(int id)
        // {
        //     return View();
        // }

        // GET: Patient/Create

        [Authorize(Roles = "Staff")]
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
            )
            {
                if ((await this._userManager.FindByNameAsync(registerDto.Email) != null)
                    && (this._patientService.Get().Any(p => p.Email.Equals(registerDto.Email))))
                {
                    TempData["ErrorMessage"] = "Email al in gebruik";
                    return RedirectToAction("Create", "Dossier");
                }

                string patientNumber = Guid.NewGuid().ToString();
                string pictureUrl = ProcessUploadedFile(picture);
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
                        IdNumber = registerDto.IdNumber
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

            var temdata = TempData;

            return RedirectToAction("Create", "Dossier");
        }

        // GET: Patient/Edit/5

        [Authorize(Roles = "Patient, Staff")]
        public async Task<ActionResult> Edit(int id)
        {
            Patient patient = await _patientService.Get(id);


            return View("Edit", this.ToDto(patient));
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient, Staff")]
        public async Task<ActionResult> Edit(int id, PatientDto patientDto, IFormFile picture)
        {
            try
            {
                if
                    (!ModelState.IsValid)
                {
                    return View(patientDto);
                }

                string pictureUrl = patientDto.Picture;
                if (picture != null)
                {
                    pictureUrl = ProcessUploadedFile(picture);
                }

                await _patientService.Update(new Patient()
                {
                    Id = patientDto.id.Value,
                    Email = patientDto.Email,
                    FirstName = patientDto.FirstName,
                    LastName = patientDto.LastName,
                    Preposition = patientDto.Preposition,
                    PhoneNumber = patientDto.PhoneNumber,
                    PictureUrl = pictureUrl,
                    BirthDay = patientDto.BirthDay,
                    Gender = patientDto.Gender,
                    PatientNumber = patientDto.ToString(),
                    IdNumber = patientDto.IdNumber
                });
                TempData["SuccessMessage"] = $"Patient Bijgewerkt";
                return RedirectToAction("Index");
            }
            catch
            {
                return View("_Edit", patientDto);
            }
        }


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

        private PatientDto ToDto(Patient patient)
        {
            PatientDto patientDto = new PatientDto()
            {
                id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Preposition = patient.Preposition,
                Email = patient.Email,
                PatientNumber = patient.PatientNumber,
                IdNumber = patient.IdNumber,
                Picture = patient.PictureUrl,
                PhoneNumber = patient.PhoneNumber,
                BirthDay = patient.BirthDay,
                Gender = patient.Gender
            };
            return patientDto;
        }
    }
}