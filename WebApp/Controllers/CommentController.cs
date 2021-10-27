using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Dtos.Comment;

namespace WebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly IService<Dossier> _dossierService;
        private readonly IService<Comment> _commentService;
        private readonly IUserService _userService;


        public CommentController(IService<Dossier> dossierService, IService<Comment> commentService,
            IUserService userService)
        {
            _dossierService = dossierService;
            _commentService = commentService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [Route("Comment/Create/{dossierId}")]
        public ActionResult Create([FromRoute] int dossierId)
        {
            CreateCommentDto createCommentDto = new CreateCommentDto()
            {
                DossierId = dossierId
            };

            return View(createCommentDto);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create(CreateCommentDto createCommentDto)
        {   
            if (ModelState.IsValid)
            {
                User user = _userService.Get(u => u.Email.Equals(User.Identity.Name)).First();
                Dossier dossier = await _dossierService.Get(createCommentDto.DossierId);
                Comment comment = await _commentService.Add(new Comment()
                {
                    CommentBody = createCommentDto.CommentBody,
                    CreatedBy = user,
                    isPostedOn = dossier,
                    IsVisiblePatient = createCommentDto.IsVisiblePatient
                });
                
//                 dossier
// .Comments.ToList().Add(comment);
                await _dossierService.Update(dossier);
                TempData["SuccessMessage"] = "Success";
                return RedirectToAction("Detail", "Dossier",createCommentDto.DossierId);
            }
                
            return View(createCommentDto);
        }
    }
}