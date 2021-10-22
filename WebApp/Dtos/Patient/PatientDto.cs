using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Domain.Enums;

namespace WebApp.Dtos.Models
{
    public class PatientDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? Preposition { get; set; }
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
    }
}