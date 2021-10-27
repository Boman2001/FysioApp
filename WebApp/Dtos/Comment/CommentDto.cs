using System.ComponentModel.DataAnnotations;
using Core.Domain.Models;

namespace WebApp.Dtos.Comment
{
    public class CommentDto
    {
        [Required]
        [Display(Name = "Opmerking")]
        public string CommentBody { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
    }
}