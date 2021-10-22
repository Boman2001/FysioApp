using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Dossier;
using WebApp.Models.Dossier;

namespace WebApp.Controllers
{
    public class DossierController : Controller
    {
        private readonly IRepository<Doctor> _DoctorRepository;
        private readonly IRepository<Student> _StudentRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<DiagnoseCode> _diagnoseRepository;

        public DossierController(IRepository<Doctor> doctorRepository, IRepository<Student> studentRepository,
            IRepository<Patient> patientRepository, IRepository<DiagnoseCode> diagnoseRepository)
        {
            _DoctorRepository = doctorRepository;
            _StudentRepository = studentRepository;
            _patientRepository = patientRepository;
            _diagnoseRepository = diagnoseRepository;
        }


        // GET: Patient/Create
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create()
        {
            CreateDossierDto viewModel = new CreateDossierDto();
            IEnumerable<Doctor> doctors = _DoctorRepository.Get();
            IEnumerable<Student> students = _StudentRepository.Get();
            IEnumerable<User> users = new List<User>();
            IEnumerable<Patient> patients = _patientRepository.Get();
            IEnumerable<DiagnoseCode> diagnoseCodes = await _diagnoseRepository.GetAsync();
            users = users.Union(students).Union(doctors);
            viewModel.Patients = new List<SelectListItem>();
            viewModel.Staff = new List<SelectListItem>();
            viewModel.Diagnoses = new List<SelectListItem>();
            foreach (var doctor in users)
            {
                viewModel.Staff.Add(
                    new SelectListItem(doctor.LastName + " , " + doctor.FirstName, doctor.Id.ToString()));
            }
            
            foreach (var patient in patients)
            {
                viewModel.Patients.Add(
                    new SelectListItem(patient.LastName + " , " + patient.FirstName, patient.Id.ToString()));
            }

            diagnoseCodes.ForEach(dc =>
            {
                viewModel.Diagnoses.Add(
                    new SelectListItem(dc.Code + " , " + dc.Pathology +" "+ dc.LocationBody, dc.Id.ToString()));
            });
            return View(viewModel);
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public Task<ActionResult> Create(CreateDossierDto dossier)
        {
            return null;
        }

        [HttpGet]
        public ActionResult Doctors()
        {
            DossierCreateViewModel viewModel = new DossierCreateViewModel();
            IEnumerable<Doctor> doctors = _DoctorRepository.Get();
            IEnumerable<Patient> patients = _patientRepository.Get();
            viewModel.Staff = new List<SelectListItem>();
            viewModel.Patients = new List<SelectListItem>();

            foreach (var doctor in doctors)
            {
                viewModel.Staff.Add(
                    new SelectListItem(doctor.LastName + " , " + doctor.FirstName, doctor.Id.ToString()));
            }
            foreach (var patient in patients)
            {
                viewModel.Patients.Add(
                    new SelectListItem(patient.LastName + " , " + patient.FirstName, patient.Id.ToString()));
            }

            return PartialView("_EmployeeDropdown", doctors);
        }

        [HttpGet]
        public ActionResult Students()
        {
            IEnumerable<Student> students = _StudentRepository.Get();
            return PartialView("_EmployeeDropdown", students);
        }

        [HttpGet]
        public ActionResult Patients()
        {
            IEnumerable<Doctor> doctors = _DoctorRepository.Get();
            return PartialView("_EmployeeDropdown", doctors);
        }
    }
}