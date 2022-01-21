using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationServices.ExtensionMethods;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Appointment;
using WebApp.Dtos.Treatment;

namespace WebApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IUserService _userService;
        private readonly IService<Dossier> _dossierService;
        private readonly IService<Appointment> _appointmentService;
        private readonly IService<Treatment> _treatmentService;

        public AppointmentController(IUserService userService, IService<Dossier> dossierService,
            IService<Appointment> appointmentService, IService<Treatment> treatmentService)
        {
            _userService = userService;
            _dossierService = dossierService;
            _appointmentService = appointmentService;
            _treatmentService = treatmentService;
        }

        // GET
        public IActionResult Index( [FromQuery] DateTime day)
        {
            List<Appointment> appointments = new List<Appointment>();
            List<Treatment> treatments = new List<Treatment>();
            if (User.IsInRole("Staff"))
            {
                appointments =
                    _appointmentService.Get(a => a.ExcecutedBy.Email == User.Identity.Name).ToList();

                treatments =
                    _treatmentService.Get(a => a.ExcecutedBy.Email == User.Identity.Name).ToList();
            }
            else
            {
                appointments =
                    _appointmentService.Get(a => a.Dossier.Patient.Email == User.Identity.Name).ToList();

                treatments =
                    _treatmentService.Get(a => a.Dossier.Patient.Email == User.Identity.Name).ToList();
            }

            List<AppointmentViewDto> appointmentViewDtos = new List<AppointmentViewDto>();
            List<TreatmentViewDto> treatmentViewDtos = new List<TreatmentViewDto>();
            appointments.ForEach(t =>
            {
                appointmentViewDtos.Add(new AppointmentViewDto()
                {
                    Practicioner = t.ExcecutedBy,
                    Room = t.Room,
                    TreatmentDate = t.TreatmentDate,
                    TreatmentEndDate = t.TreatmentDate.AddMinutes(t.Dossier.TreatmentPlan.TimePerSessionInMinutes),
                    PracticionerId = t.ExcecutedBy.Id,
                    Patient = t.Dossier.Patient,
                    Id = t.Id,
                    DossierId = t.Dossier.Id
                });
            });

            treatments.ForEach(t =>
            {
                treatmentViewDtos.Add(new TreatmentViewDto()
                {
                    Practicioner = t.ExcecutedBy,
                    Room = t.Room,
                    TreatmentDate = t.TreatmentDate,
                    TreatmentEndDate = t.TreatmentDate.AddMinutes(t.Dossier.TreatmentPlan.TimePerSessionInMinutes),
                    PracticionerId = t.ExcecutedBy.Id,
                    Patient = t.Dossier.Patient,
                    Id = t.Id,
                    DossierId = t.Dossier.Id,
                    Description = t.Description,
                    TreatmentCode = t.TreatmentCode,
                    createdAt =  t.CreatedAt,
                    Particulatities = t.Particulatities

                });
            });

            if (day != DateTime.Today)
            {
                appointmentViewDtos = appointmentViewDtos.Where(dto => dto.TreatmentDate.Date == day.Date).ToList();
                treatmentViewDtos = treatmentViewDtos.Where(dto => dto.TreatmentDate.Date == day.Date).ToList();
            }
            else
            {
                appointmentViewDtos = appointmentViewDtos.Where(dto => dto.TreatmentDate.Date == DateTime.Now.Date).ToList();
                treatmentViewDtos = treatmentViewDtos.Where(dto => dto.TreatmentDate.Date == DateTime.Now.Date).ToList();

            }
            
            return View(new AppointmentIndexDto()
            {
                treatmentViewDtos = treatmentViewDtos,
                AppointmentViewDtos = appointmentViewDtos
            });
        }

        [HttpGet]
        [Authorize(Roles = "Staff, Patient")]
        [Route("Appointment/Create/{dossierId}")]
        public async Task<ActionResult> Create([FromRoute] int dossierId)
        {
            Dossier dossier = await _dossierService.Get(dossierId);
            List<User> users = new List<User>();
            if (User.IsInRole("Staff"))
            {
                users =  _userService.GetStaff().ToList();
            }
            else
            {
                users.Add(dossier.HeadPractitioner);
            }

            List<SelectListItem> StaffList = new List<SelectListItem>();

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    StaffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }


            CreateAppointmentDto viewModel = new CreateAppointmentDto()
            {
                Staff = StaffList,
                Dossier = dossier,
                TreatmentPlan = dossier.TreatmentPlan

            };
            viewModel.DossierId = dossierId;


            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff, Patient")]
        public async Task<ActionResult> Create(CreateAppointmentDto treatmentDto)
        {
            Dossier dossier = await _dossierService.Get(treatmentDto.DossierId);
            if (ModelState.IsValid)
            {
                Staff user = (Staff) await _userService.Get(treatmentDto.PracticionerId);
                try
                {
                    await _appointmentService.Add(new Appointment()
                    {
                        TreatmentDate = treatmentDto.TreatmentDate,
                        Room = treatmentDto.Room,
                        Dossier = dossier,
                        ExcecutedBy = user,
                    });
                    TempData["SuccessMessage"] = "Success";
                    if (User.IsInRole("Staff"))
                    {
                        return RedirectToAction("Index", "Dossier");

                    }
                    return RedirectToAction("Details", "Dossier");
                }
                catch (ValidationException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                }
            }

            IEnumerable<User> users = _userService.GetStaff();

            List<SelectListItem> StaffList = new List<SelectListItem>();

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    StaffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }

            treatmentDto.Staff = StaffList;
            treatmentDto.Dossier = dossier;
            treatmentDto.TreatmentPlan = dossier.TreatmentPlan;
            ViewBag.error = "Something went wrong, please try again later";
            return View("Create", treatmentDto);
        }
        
        
        [HttpGet]
        [Authorize(Roles = "Staff, Patient")]
        [Route("Appointment/edit/{appointmentId}")]
        public async Task<ActionResult> Edit([FromRoute] int appointmentId)
        {
            Appointment appointment = await _appointmentService.Get(appointmentId);
            if (appointment == null)
            {
                appointment = (Appointment) await _treatmentService.Get(appointmentId);
            }
            Dossier dossier = appointment.Dossier;
            List<User> users = new List<User>();
            if (User.IsInRole("Staff"))
            {
                users =  _userService.GetStaff().ToList();
            }
            else
            {
                users.Add(dossier.HeadPractitioner);
            }

            List<SelectListItem> StaffList = new List<SelectListItem>();

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    StaffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }


            CreateAppointmentDto viewModel = new CreateAppointmentDto()
            {
                Id = appointmentId,
                Staff = StaffList,
                PracticionerId = appointment.ExcecutedBy.Id,
                Room = appointment.Room,
                DossierId = appointment.Dossier.Id,
                TreatmentDate = appointment.TreatmentDate
            };


            return View("Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff, Patient")]
        public async Task<ActionResult> Edit(CreateAppointmentDto treatmentDto)
        {
            Dossier dossier = await _dossierService.Get(treatmentDto.DossierId);
            if (ModelState.IsValid)
            {
                Staff user = (Staff) await _userService.Get(treatmentDto.PracticionerId);
                try
                {
                    await _appointmentService.Update(treatmentDto.Id,new Appointment()
                    {
                        TreatmentDate = treatmentDto.TreatmentDate,
                        Room = treatmentDto.Room,
                        Dossier = dossier,
                        ExcecutedBy = user,
                    });
                    TempData["SuccessMessage"] = "Success";
                    return RedirectToAction("Index", "Appointment");
                }
                catch (ValidationException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                }
            }

            IEnumerable<User> users = _userService.GetStaff();

            List<SelectListItem> StaffList = new List<SelectListItem>();

            if (users.ToList().Count > 0)
            {
                users.ForEach(u =>
                {
                    StaffList.Add(new SelectListItem(u.GetFormattedName(), u.Id.ToString(),
                        u.Id == dossier.HeadPractitioner.Id));
                });
            }

            treatmentDto.Staff = StaffList;
            ViewBag.error = "Something went wrong, please try again later";
            return View("Edit", treatmentDto);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Appointments([FromQuery(Name = "id")]int id ,[FromQuery(Name = "time")] string time)
        {
            DateTime date = DateTime.Parse(time);
            List<Appointment> appointments = new List<Appointment>();
            appointments = _appointmentService.Get(a => a.ExcecutedBy.Id == id && a.TreatmentDate.Equals(date)).ToList();
            List<AppointmentViewDto> appointmentViewDtos = new List<AppointmentViewDto>();
            appointments.ForEach(t =>
            {
                appointmentViewDtos.Add(new AppointmentViewDto()
                {
                    Practicioner = t.ExcecutedBy,
                    Room = t.Room,
                    TreatmentDate = t.TreatmentDate,
                    PracticionerId = t.ExcecutedBy.Id,
                    Patient = t.Dossier.Patient,
                    Id = t.Id,
                    DossierId = t.Dossier.Id
                });
            });
            return PartialView("_Appointments", appointmentViewDtos  );
        }
    }
}