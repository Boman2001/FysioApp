using System.ComponentModel.DataAnnotations;

namespace WebApp.Dtos.Comment
{
    public class CreateCommentDto : CommentDto
    {
        
        public int DossierId { get; set; }
        [Required]
        [Display(Name = "Zichtbaar Patient")]
        public bool IsVisiblePatient { get; set; }
    }
}