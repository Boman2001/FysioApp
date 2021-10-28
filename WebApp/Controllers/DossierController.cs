using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Infrastructure.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Appointment;
using WebApp.Dtos.Comment;
using WebApp.Dtos.Dossier;
using WebApp.Dtos.Treatment;
using WebApp.Dtos.TreatmentPlan;
using WebApp.helpers;
using WebApp.Models.Dossier;

namespace WebApp.Controllers
{
    public class DossierController : Controller
    {
        private readonly IWebRepository<TreatmentCode> _treatmentCodeRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IWebRepository<DiagnoseCode> _diagnoseRepository;
        private readonly IService<Dossier> _dossierService;
        private readonly IRepository<TreatmentPlan> _treatmentPlanRepository;

        public DossierController(IWebRepository<TreatmentCode> treatmentCodeRepository,
            IRepository<Doctor> doctorRepository, IRepository<Student> studentRepository,
            IRepository<User> userRepository, IRepository<Patient> patientRepository,
            IWebRepository<DiagnoseCode> diagnoseRepository, IService<Dossier> dossierService,
            IRepository<TreatmentPlan> treatmentPlanRepository)
        {
            _treatmentCodeRepository = treatmentCodeRepository;
            _doctorRepository = doctorRepository;
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _diagnoseRepository = diagnoseRepository;
            _dossierService = dossierService;
            _treatmentPlanRepository = treatmentPlanRepository;
        }

        // GET: Patient/Create
        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            IEnumerable<Dossier> dossiers = _dossierService.Get();
            return View(dossiers);
        }

        // GET: Patient/Create
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create()
        {
            CreateDossierDto viewModel = await this.fillDto(new CreateDossierDto());


            return View(viewModel);
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create(CreateDossierDto dossier)
        {
            if (ModelState.IsValid)
            {
                Patient patient = await _patientRepository.Get(dossier.PatientId);
                if (patient != null)
                {
                    User head = await _userRepository.Get(dossier.HeadPracticionerId);
                    User intakeBy = await _userRepository.Get(dossier.IntakeById);
#nullable enable
                    User? supervisor = null;
#nullable disable
                    if (dossier.SupervisedById.HasValue)
                    {
                        supervisor = await _userRepository.Get(dossier.SupervisedById.Value);
                    }

                    TreatmentPlan treatmentplan = await _treatmentPlanRepository.Add(new TreatmentPlan()
                    {
                        TreatmentsPerWeek = dossier.TreatmentsPerWeek,
                        TimePerSessionInMinutes = dossier.TimePerSessionInMinutes
                    });

                    Dossier dossierEnt = new Dossier()
                    {
                        Patient = patient,
                        DiagnoseCodeId = dossier.DiagnoseCodeId,
                        Description = dossier.Description,
                        IsStudent = dossier.IsStudent,
                        RegistrationDate = dossier.AdmissionDate,
                        HeadPractitioner = head,
                        SupervisedBy = supervisor,
                        IntakeBy = intakeBy,
                        TreatmentPlan = treatmentplan,
                    };

                    try
                    {
                        Dossier result = await _dossierService.Add(dossierEnt);
                        TempData["SuccessMessage"] =
                            $"{result.Patient.FirstName} {result.Patient.Preposition} {result.Patient.LastName}'s Dossier is aangemaakt";
                        return RedirectToAction("Index", "Home");
                    }
                    catch (ValidationException e)
                    {
                        TempData["ErrorMessage"] = e.Message;
                    }
                }
            }

            CreateDossierDto viewModel = await this.fillDto(dossier);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Doctors()
        {
            DossierCreateViewModel viewModel = new DossierCreateViewModel();
            IEnumerable<Doctor> doctors = _doctorRepository.Get();
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
            IEnumerable<Student> students = _studentRepository.Get();
            return PartialView("_EmployeeDropdown", students);
        }

        [HttpGet]
        public ActionResult Patients()
        {
            IEnumerable<Doctor> doctors = _doctorRepository.Get();
            return PartialView("_EmployeeDropdown", doctors);
        }

        private async Task<CreateDossierDto> fillDto(CreateDossierDto viewModel)
        {
            try
            {
                IEnumerable<Doctor> doctors = _doctorRepository.Get();
                IEnumerable<Student> students = _studentRepository.Get();
                IEnumerable<User> users = new List<User>();
                IEnumerable<Patient> patients = _patientRepository.Get();
                IEnumerable<DiagnoseCode> diagnoseCodes = await _diagnoseRepository.GetAsync();
                IEnumerable<TreatmentCode> treatments = await _treatmentCodeRepository.GetAsync();

                //TODO haal dit op via user service en check op !patient
                users = users.Union(students).Union(doctors);
                viewModel.Patients = new List<SelectListItem>();
                viewModel.Staff = new List<SelectListItem>();
                viewModel.Diagnoses = new List<SelectListItem>();
                viewModel.TreatmentItems = new List<SelectListItem>();


                if (treatments != null)
                {
                    treatments.ForEach(code =>
                    {
                        viewModel.TreatmentItems.Add(new SelectListItem(code.Code + " , " + code.Description,
                            code.Id.ToString()));
                    });
                }

                users.ForEach(doctor =>
                {
                    viewModel.Staff.Add(
                        new SelectListItem(doctor.LastName + " , " + doctor.FirstName, doctor.Id.ToString()));
                });

                patients.ForEach(patient =>
                {
                    viewModel.Patients.Add(
                        new SelectListItem(patient.LastName + " , " + patient.FirstName, patient.Id.ToString()));
                });


                diagnoseCodes.ForEach(dc =>
                {
                    viewModel.Diagnoses.Add(
                        new SelectListItem(dc.Code + " , " + dc.Pathology + " " + dc.LocationBody, dc.Id.ToString()));
                });
                return viewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Edit(CreateDossierDto dossier)
        {
            if (ModelState.IsValid)
            {
                Patient patient = await _patientRepository.Get(dossier.PatientId);
                if (patient != null)
                {
                    User head = await _userRepository.Get(dossier.HeadPracticionerId);
                    User intakeBy = await _userRepository.Get(dossier.IntakeById);
#nullable enable
                    User? supervisor = null;
#nullable disable
                    if (dossier.SupervisedById.HasValue)
                    {
                        supervisor = await _userRepository.Get(dossier.SupervisedById.Value);
                    }

                    TreatmentPlan treatmentplan = await _treatmentPlanRepository.Add(new TreatmentPlan()
                    {
                        TreatmentsPerWeek = dossier.TreatmentsPerWeek,
                        TimePerSessionInMinutes = dossier.TimePerSessionInMinutes
                    });

                    var dossierEnt = new Dossier()
                    {
                        Patient = patient,
                        DiagnoseCodeId = dossier.DiagnoseCodeId,
                        Description = dossier.Description,
                        IsStudent = dossier.IsStudent,
                        RegistrationDate = dossier.AdmissionDate,
                        HeadPractitioner = head,
                        SupervisedBy = supervisor,
                        IntakeBy = intakeBy,
                        TreatmentPlan = treatmentplan
                    };

                    try
                    {
                        Dossier result = await _dossierService.Add(dossierEnt);
                        TempData["SuccessMessage"] =
                            $"{result.Patient.FirstName} {result.Patient.Preposition} {result.Patient.LastName}'s Dossier is aangemaakt";
                        return RedirectToAction("Index", "Home");
                    }
                    catch (ValidationException e)
                    {
                        TempData["ErrorMessage"] = e.Message;
                    }
                }
            }

            CreateDossierDto viewModel = await this.fillDto(dossier);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Detail([FromRoute] int id)
        {
            List<ViewDossierDto> dossierDtos = new List<ViewDossierDto>();
            Patient patient = await _patientRepository.Get(id);
            patient.Dossiers.ForEach(dossier =>
            {
                IEnumerable<TreatmentCode> treatmentcodes =  _treatmentCodeRepository.GetAsync().Result;
                List<ViewCommentDto> commentDtos = new List<ViewCommentDto>();
                List<TreatmentViewDto> treatmentViewDtos = new List<TreatmentViewDto>();
                DiagnoseCode diagnoseCode =  _diagnoseRepository.Get(dossier.DiagnoseCodeId).Result;
                dossier.Comments.ForEach(c =>
                {
                    commentDtos.Add(new ViewCommentDto()
                    {
                        CommentBody = c.CommentBody,
                        CreatedBy = c.CreatedBy,
                        IsVisiblePatient = c.IsVisiblePatient,
                        CreatedById = c.CreatedBy.Id
                    });
                });

                dossier.Treatments.ForEach(t =>
                {
                    var treatmentCode = treatmentcodes.First(tc => t.TreatmentCodeId == tc.Id);
                    treatmentViewDtos.Add(new TreatmentViewDto()
                    {
                        Description = t.Description,
                        Practicioner = t.ExcecutedBy,
                        TreatmentCode = treatmentCode,
                        Room = t.Room,
                        TreatmentDate = t.TreatmentDate,
                        PracticionerId = t.ExcecutedBy.Id,
                        TreatmentCodeId = t.TreatmentCodeId.Value,
                        Id = t.Id
                    });
                });

                dossier.Appointments.ForEach(t =>
                {
                    treatmentViewDtos.Add(new TreatmentViewDto()
                    {
                        Practicioner = t.ExcecutedBy,
                        Room = t.Room,
                        TreatmentDate = t.TreatmentDate,
                        PracticionerId = t.ExcecutedBy.Id,
                        Id = t.Id
                    });
                });
                ViewDossierDto dossierDto = new ViewDossierDto()
                {
                    Patient = dossier.Patient,
                    DiagnoseCodeId = dossier.DiagnoseCodeId,
                    Description = dossier.Description,
                    IsStudent = dossier.IsStudent,
                    AdmissionDate = dossier.RegistrationDate,
                    HeadPractitioner = dossier.HeadPractitioner,
                    SupervisedBy = dossier.SupervisedBy,
                    IntakeBy = dossier.IntakeBy,
                    TreatmentPlan = new TreatmentPlanDto()
                    {
                        TreatmentsPerWeek = dossier.TreatmentPlan.TreatmentsPerWeek,
                        TimePerSessionInMinutes = dossier.TreatmentPlan.TimePerSessionInMinutes
                    },
                    Comments = commentDtos,
                    Treatments = treatmentViewDtos,
                    DiagnoseCode = diagnoseCode,
                    Age = dossier.Age,
                    Id = dossier.Id
                };
                dossierDtos.Add(dossierDto);
            });


            return View(dossierDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Patient")]
        [Route("Dossiers")]
        public async Task<ActionResult> Details([FromQuery] string email)
        {

            List<ViewDossierDto> dossierDtos = new List<ViewDossierDto>();
            Patient patient = _patientRepository.Get(p => p.Email == email).First();
            IEnumerable<TreatmentCode> treatmentcodes = await _treatmentCodeRepository.GetAsync();

            patient.Dossiers.ForEach(dossier =>
            {
                List<ViewCommentDto> commentDtos = new List<ViewCommentDto>();
                List<AppointmentViewDto> appointmentViewDtos = new List<AppointmentViewDto>();
                DiagnoseCode diagnoseCode = _diagnoseRepository.Get(dossier.DiagnoseCodeId).Result;
                dossier.Comments.Where(c => c.IsVisiblePatient).ForEach(c =>
                {
                    commentDtos.Add(new ViewCommentDto()
                    {
                        CommentBody = c.CommentBody,
                        CreatedBy = c.CreatedBy,
                        IsVisiblePatient = c.IsVisiblePatient,
                        CreatedById = c.CreatedBy.Id
                    });
                });

                dossier.Appointments.ForEach(t =>
                {
                    appointmentViewDtos.Add(new AppointmentViewDto()
                    {
                        Practicioner = t.ExcecutedBy,
                        Room = t.Room,
                        TreatmentDate = t.TreatmentDate,
                        PracticionerId = t.ExcecutedBy.Id,
                        Patient = t.Dossier.Patient,
                        Id = t.Id
                    });
                });

                dossier.Treatments.ForEach(t =>
                {
                    appointmentViewDtos.Add(new AppointmentViewDto()
                    {
                        Practicioner = t.ExcecutedBy,
                        Room = t.Room,
                        TreatmentDate = t.TreatmentDate,
                        PracticionerId = t.ExcecutedBy.Id,
                        Patient = t.Dossier.Patient,
                        Id = t.Id
                    });
                });
                ViewDossierDto dossierDto = new ViewDossierDto()
                {
                    Patient = dossier.Patient,
                    DiagnoseCodeId = dossier.DiagnoseCodeId,
                    Description = dossier.Description,
                    IsStudent = dossier.IsStudent,
                    AdmissionDate = dossier.RegistrationDate,
                    HeadPractitioner = dossier.HeadPractitioner,
                    SupervisedBy = dossier.SupervisedBy,
                    IntakeBy = dossier.IntakeBy,
                    TreatmentPlan = new TreatmentPlanDto()
                    {
                        TreatmentsPerWeek = dossier.TreatmentPlan.TreatmentsPerWeek,
                        TimePerSessionInMinutes = dossier.TreatmentPlan.TimePerSessionInMinutes
                    },
                    Comments = commentDtos,
                    Appointments = appointmentViewDtos,
                    DiagnoseCode = diagnoseCode,
                    Age = dossier.Age,
                    Id = dossier.Id
                };
                dossierDtos.Add(dossierDto);
                
            });
            return View("Detail", dossierDtos);
        }
    }
}