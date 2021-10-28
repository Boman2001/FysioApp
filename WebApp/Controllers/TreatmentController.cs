using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationServices.ExtensionMethods;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Infrastructure.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Treatment;

namespace WebApp.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly IWebRepository<TreatmentCode> _treatmentCodeRepository;
        private readonly IRepository<TreatmentPlan> _treatmentPlanService;
        private readonly IService<Treatment> _treatmentService;
        private readonly IService<Appointment> _appointmentService;
        private readonly IService<Dossier> _dossierService;
        private readonly IUserService _userRepository;

        public TreatmentController(IWebRepository<TreatmentCode> treatmentCodeRepository, IRepository<TreatmentPlan> treatmentPlanService, IService<Treatment> treatmentService, IService<Appointment> appointmentService, IService<Dossier> dossierService, IUserService userRepository)
        {
            _treatmentCodeRepository = treatmentCodeRepository;
            _treatmentPlanService = treatmentPlanService;
            _treatmentService = treatmentService;
            _appointmentService = appointmentService;
            _dossierService = dossierService;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [Route("Treatment/Create/{dossierId}")]
        public async Task<ActionResult> Create([FromRoute] int dossierId)
        {
            IEnumerable<TreatmentCode> treatments = await _treatmentCodeRepository.GetAsync();
            Dossier dossier = await _dossierService.Get(dossierId);
            IEnumerable<User> users = _userRepository.GetStaff();
            var treatmentCodeList = new List<SelectListItem>();
            var staffList = new List<SelectListItem>();
            
            if (treatments != null)
            {
                treatments.ForEach(code =>
                {
                    treatmentCodeList.Add(new SelectListItem(code.Code + " , " + code.Description,
                        code.Id.ToString()));
                });
            }

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    staffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }


            CreateTreatmentDto viewModel = new CreateTreatmentDto()
            {
                Treatments = treatmentCodeList,
                Staff = staffList
            };
            viewModel.DossierId = dossierId;


            return View("Create", viewModel);
        }
        
        [HttpGet]
        [Authorize(Roles = "Staff")]
        [Route("Treatment/Create/Appointment/{AppointmentId}")]
        public async Task<ActionResult> CreateFromAppointment([FromRoute] int AppointmentId)
        {
            
            IEnumerable<TreatmentCode> treatments = await _treatmentCodeRepository.GetAsync();
            Appointment appointment = await _appointmentService.Get(AppointmentId);
            Dossier dossier = appointment.Dossier;
            IEnumerable<User> users = _userRepository.GetStaff();
            var treatmentCodeList = new List<SelectListItem>();
            var staffList = new List<SelectListItem>();


            if (treatments != null)
            {
                treatments.ForEach(code =>
                {
                    treatmentCodeList.Add(new SelectListItem(code.Code + " , " + code.Description,
                        code.Id.ToString()));
                });
            }

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    staffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }


            CreateTreatmentDto viewModel = new CreateTreatmentDto()
            {
                Treatments = treatmentCodeList,
                Staff = staffList,
                PracticionerId = appointment.ExcecutedBy.Id,
                TreatmentDate = appointment.TreatmentDate
            };
            viewModel.DossierId = dossier.Id;


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
                Staff user = (Staff) await _userRepository.Get(treatmentDto.PracticionerId);
                try
                {
                    Appointment appointmentToDelete = _appointmentService.Get(a =>
                        a.TreatmentDate == treatmentDto.TreatmentDate && a.Dossier.Id == dossier.Id).FirstOrDefault();
                    if (appointmentToDelete != null)
                    {
                        await _appointmentService.Delete(appointmentToDelete);
                    }
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
            IEnumerable<User> users = _userRepository.GetStaff();
            var treatmentCodeList = new List<SelectListItem>();
            var staffList = new List<SelectListItem>();
            
            if (treatments != null)
            {
                treatments.ForEach(code =>
                {
                    treatmentCodeList.Add(new SelectListItem(code.Code + " , " + code.Description,
                        code.Id.ToString()));
                });
            }

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    staffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }

            treatmentDto.Staff = staffList;
            treatmentDto.Treatments = treatmentCodeList;
            ViewBag.error = "Something went wrong, please try again later";
            return View("Create", treatmentDto);
        }
        
        [HttpGet]
        [Authorize(Roles = "Staff")]
        [Route("Treatment/Edit/{treatmentId}")]
        public async Task<ActionResult> Edit([FromRoute] int treatmentId)
        {
            Treatment treatment = await _treatmentService.Get(treatmentId);
            IEnumerable<TreatmentCode> treatments = await _treatmentCodeRepository.GetAsync();
            IEnumerable<User> users = _userRepository.GetStaff();
            var treatmentCodeList = new List<SelectListItem>();
            var staffList = new List<SelectListItem>();
            
            if (treatments != null)
            {
                treatments.ForEach(code =>
                {
                    treatmentCodeList.Add(new SelectListItem(code.Code + " , " + code.Description,
                        code.Id.ToString(), code.Id == treatment.TreatmentCodeId ));
                });
            }

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    staffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == treatment.Dossier.HeadPractitioner.Id));
                });
            }


            CreateTreatmentDto viewModel = new CreateTreatmentDto()
            {
                Treatments = treatmentCodeList,
                Staff = staffList,
                Description = treatment.Description,
                Dossier = treatment.Dossier,
                DossierId = treatment.Dossier.Id,
                Particulatities = treatment.Particulatities,
                Room = treatment.Room,
                TreatmentCode = treatments.First(t => t.Id == treatment.TreatmentCodeId),
                TreatmentDate = treatment.TreatmentDate
                
            };
            viewModel.DossierId = treatment.Dossier.Id;


            return View("Edit", viewModel);
        }
        
                [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Edit(CreateTreatmentDto treatmentDto)
        {
            Dossier dossier = await _dossierService.Get(treatmentDto.DossierId);
            if (ModelState.IsValid)
            {
                TreatmentCode treatmentCode = await _treatmentCodeRepository.Get(treatmentDto.TreatmentCodeId);
                Staff user = (Staff) await _userRepository.Get(treatmentDto.PracticionerId);
                try
                {
                    await _treatmentService.Update(new Treatment()
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
            IEnumerable<User> users = _userRepository.GetStaff();
            var treatmentCodeList = new List<SelectListItem>();
            var staffList = new List<SelectListItem>();
            
            if (treatments != null)
            {
                treatments.ForEach(code =>
                {
                    treatmentCodeList.Add(new SelectListItem(code.Code + " , " + code.Description,
                        code.Id.ToString()));
                });
            }

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    staffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }

            treatmentDto.Staff = staffList;
            treatmentDto.Treatments = treatmentCodeList;
            ViewBag.error = "Something went wrong, please try again later";
            return View("Create", treatmentDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]

        public async Task<ActionResult> Delete(int id)
        {
           await _treatmentService.Delete(id);
           return RedirectToAction("Index","Appointment");
        }
    }
}