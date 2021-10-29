using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.Dtos.Auth
{
    public class EditStudentDto : StaffEditDto
    {
        [NotNull]        [Required]
        public string StudentNumber { get; set; }
        
    }
}