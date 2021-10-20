using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.Dtos.Auth
{
    public class DoctorRegisterDto : RegisterDto
    {
        [NotNull]
        public string BigNumber { get; set; }
        [NotNull]
        public string EmployeeNumber { get; set; }
        [NotNull]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}