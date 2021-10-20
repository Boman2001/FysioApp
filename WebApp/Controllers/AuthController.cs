// using System.Security.Claims;
// using System.Threading.Tasks;
// using Core.Domain.Models;
// using Core.DomainServices.Interfaces;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using WebApp.Dtos;
//
// namespace Web.Controllers
// {
//     public class AuthController : Controller
//     {
//         private readonly UserManager<IdentityUser> userManager;
//         private readonly SignInManager<IdentityUser> signInManager;
//         private readonly IRepository<Doctor> _DoctorRepository;
//         private readonly IRepository<Student> _StudentRepository;
//
//         public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
//             IRepository<Doctor> doctorRepository, IRepository<Student> studentRepository)
//         {
//             this.userManager = userManager;
//             this.signInManager = signInManager;
//             _DoctorRepository = doctorRepository;
//             _StudentRepository = studentRepository;
//         }
//
//         [HttpGet]
//         [AllowAnonymous]
//         public IActionResult Index()
//         {
//             return RedirectToActionPermanent("Login");
//         }
//
//         [HttpGet]
//         [AllowAnonymous]
//         public IActionResult Login()
//         {
//             return View();
//         }
//
//         [HttpGet]
//         [AllowAnonymous]
//         public IActionResult Register()
//         {
//             return View();
//         }
//
//         [HttpPost]
//         [Authorize]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> PostLogout()
//         {
//             await this.signInManager.SignOutAsync();
//
//             return this.RedirectToAction("Index", "Index");
//         }
//
//         [HttpPost]
//         [AllowAnonymous]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> PostRegister(RegisterDto registerDto)
//         {
//             if
//             (
//                 ModelState.IsValid
//                 && (await this.userManager.FindByNameAsync(registerDto.Email) == null)
//             )
//             {
//                 IdentityUser user = new IdentityUser(registerDto.Email) {Email = registerDto.Email};
//                 await this.userManager.CreateAsync(user, registerDto.Password);
//
//                 switch (registerDto.PatientType)
//                 {
//                     case PatientType.STUDENT_PATIENT:
//                         await this.studentPatientRepository.Create(new StudentPatientDTO()
//                         {
//                             Email = registerDto.Email,
//                             FirstName = registerDto.FirstName,
//                             LastName = registerDto.LastName,
//                             PhoneNumber = registerDto.PhoneNumber,
//                             DateOfBirth = registerDto.DateOfBirth,
//
//                             ImageUrl = "",
//                             ImagePath = "",
//                             PatientNumber = "",
//                             StudentNumber = registerDto.IdentificationNumber
//                         });
//                         await this.userManager.AddClaimAsync(user, new Claim("StudentPatient", "true"));
//                         break;
//
//                     case PatientType.FACULTY_PATIENT:
//                         await this.facultyPatientRepository.Create(new FacultyPatientDTO()
//                         {
//                             Email = registerDto.Email,
//                             FirstName = registerDto.FirstName,
//                             LastName = registerDto.LastName,
//                             PhoneNumber = registerDto.PhoneNumber,
//                             DateOfBirth = registerDto.DateOfBirth,
//
//                             ImageUrl = "",
//                             ImagePath = "",
//                             PatientNumber = "",
//                             EmployeeNumber = registerDto.IdentificationNumber
//                         });
//                         await this.userManager.AddClaimAsync(user, new Claim("FacultyPatient", "true"));
//                         break;
//                 }
//
//                 return this.RedirectToAction("PostLogin", new LoginViewModel()
//                 {
//                     Email = registerDto.Email,
//                     Password = registerDto.Password
//                 });
//             }
//
//             ViewBag.error = "Something went wrong, please try again later";
//             return this.RedirectToAction("Register");
//         }
//
//         [HttpPost]
//         [AllowAnonymous]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> PostLogin(LoginViewModel loginViewModel)
//         {
//             if (this.ModelState.IsValid)
//             {
//                 IdentityUser user = await this.userManager.FindByEmailAsync(loginViewModel.Email);
//
//                 if (user != null)
//                 {
//                     if ((await this.signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, true))
//                         .Succeeded)
//                     {
//                         return RedirectToAction("Index", "Dashboard");
//                     }
//                 }
//             }
//
//             this.ModelState.AddModelError("", "Invalid Credentials");
//             return this.RedirectToAction("Login", loginViewModel);
//         }
//     }
// }