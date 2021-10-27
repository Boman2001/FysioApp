using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationServices.ExtensionMethods;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Infrastructure.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Dossier;
using WebApp.Dtos.Treatment;
using WebApp.Dtos.TreatmentPlan;

namespace WebApp.Controllers
{
    public class TreatmentController : Controller
    {
        private IWebRepository<TreatmentCode> _treatmentCodeRepository;
        private IRepository<TreatmentPlan> _treatmentPlanService;
        private IService<Treatment> _treatmentService;
        private IService<Dossier> _dossierService;

        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<User> _userRepository;

        public TreatmentController(IWebRepository<TreatmentCode> treatmentCodeRepository,
            IRepository<TreatmentPlan> treatmentPlanService, IService<Treatment> treatmentService,
            IService<Dossier> dossierService, IRepository<Doctor> doctorRepository,
            IRepository<Student> studentRepository, IRepository<User> userRepository)
        {
            _treatmentCodeRepository = treatmentCodeRepository;
            _treatmentPlanService = treatmentPlanService;
            _treatmentService = treatmentService;
            _dossierService = dossierService;
            _doctorRepository = doctorRepository;
            _studentRepository = studentRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [Route("Treatment/Create/{dossierId}")]
        public async Task<ActionResult> Create([FromRoute] int dossierId)
        {
            IEnumerable<TreatmentCode> treatments = await _treatmentCodeRepository.GetAsync();
            IEnumerable<User> doctors = _doctorRepository.Get();
            IEnumerable<User> students = _studentRepository.Get();
            Dossier dossier = await _dossierService.Get(dossierId);
            List<User> users = new List<User>();
            var treatmentCodeList = new List<SelectListItem>();
            var StaffList = new List<SelectListItem>();

            //TODO haal dit op uit user repo en check of he een student of doctor is
            users.AddRange(doctors);
            users.AddRange(students);
            if (treatments != null)
            {
                treatments.ForEach(code =>
                {
                    treatmentCodeList.Add(new SelectListItem(code.Code + " , " + code.Description,
                        code.Id.ToString()));
                });
            }

            if (users.Count > 0)
            {
                users.ForEach(u =>
                {
                    StaffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }


            CreateTreatmentDto viewModel = new CreateTreatmentDto()
            {
                Treatments = treatmentCodeList,
                Staff = StaffList
            };
            viewModel.DossierId = dossierId;


            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create(CreateTreatmentDto treatmentDto)
        {
            Dossier dossier = await _dossierService.Get(treatmentDto.DossierId);
            if (ModelState.IsValid)
            {
                TreatmentCode treatmentCode = await _treatmentCodeRepository.Get(treatmentDto.TreatmentCodeId);
                User user = await _userRepository.Get(treatmentDto.PracticionerId);
                try
                {
                    await _treatmentService.Add(new Treatment()
                    {
                        TreatmentDate = treatmentDto.TreatmentDate,
                        TreatmentCodeId = treatmentDto.TreatmentCodeId,
                        Description = treatmentDto.Description,
                        TreatmentCode = treatmentCode,
                        Room = treatmentDto.Room,
                        Dossier = dossier,
                        ExcecutedBy = user,
                    });
                    TempData["SuccessMessage"] = "Success";
                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                }
            }

            IEnumerable<TreatmentCode> treatments = await _treatmentCodeRepository.GetAsync();
            IEnumerable<User> doctors = _doctorRepository.Get();
            IEnumerable<User> students = _studentRepository.Get();
            List<User> users = new List<User>();
            var treatmentCodeList = new List<SelectListItem>();
            var StaffList = new List<SelectListItem>();

            //TODO haal dit op uit user repo en check of he een student of doctor is
            users.AddRange(doctors);
            users.AddRange(students);
            if (treatments != null)
            {
                treatments.ForEach(code =>
                {
                    treatmentCodeList.Add(new SelectListItem(code.Code + " , " + code.Description,
                        code.Id.ToString()));
                });
            }

            if (users.Count > 0)
            {
                users.ForEach(u =>
                {
                    StaffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }

            treatmentDto.Staff = StaffList;
            treatmentDto.Treatments = treatmentCodeList;
            ViewBag.error = "Something went wrong, please try again later";
            return View("Create", treatmentDto);
        }
    }
}