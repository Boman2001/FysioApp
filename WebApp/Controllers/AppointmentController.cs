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

        public AppointmentController(IUserService userService, IService<Dossier> dossierService, IService<Appointment> appointmentService)
        {
            _userService = userService;
            _dossierService = dossierService;
            _appointmentService = appointmentService;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }
        
          [HttpGet]
        [Authorize(Roles = "Staff, Patient")]
        [Route("Appointment/Create/{dossierId}")]
        public async Task<ActionResult> Create([FromRoute] int dossierId)
        {
            Dossier dossier = await _dossierService.Get(dossierId);
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


            CreateAppointmentDto viewModel = new CreateAppointmentDto()
            {
                Staff = StaffList
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
                User user = await _userService.Get(treatmentDto.PracticionerId);
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
                    return RedirectToAction("Index", "Home");
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
            return View("Create", treatmentDto);
        }
    }
}