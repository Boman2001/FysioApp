using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.Dtos.Auth
{
    public class EditDoctorDto : StaffEditDto
    {
        [NotNull]
        [Required]
        [StringLength(11)]
        public string BigNumber { get; set; }
        [Required]
        [StringLength(7)]
        public string EmployeeNumber { get; set; }
        [NotNull]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}