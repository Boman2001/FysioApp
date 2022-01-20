using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Core.Domain.Enums;

namespace WebApp.Dtos.Auth
{
    public class PatientRegisterDto : RegisterDto
    {
#nullable enable
        public int? Id { get; set; }
        public string? Preposition { get; set; }
#nullable disable
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string PatientNumber { get; set; }
        [Required]
        [Display(Name = "Student Or Employee Number")]
        [StringLength(7)]
        public string IdNumber { get; set; }
        public string Picture { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public Gender Gender { get; set; }
        
        [Required] [Display(Name = "Straat")] public string Street { get; set; }
        [Required] [Display(Name = "Stad")] public string City { get; set; }

        [Required]
        [Display(Name = "Post Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Huisnummer en toevoeging")]
        public string HouseNumber { get; set; }
    }
}