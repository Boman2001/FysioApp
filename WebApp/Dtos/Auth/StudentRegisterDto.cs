using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.Dtos.Auth
{
    public class StudentRegisterDto : StaffDto
    {
        [NotNull]        [Required]
        public string StudentNumber { get; set; }
    }
}