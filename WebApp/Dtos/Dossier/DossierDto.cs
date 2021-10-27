using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Models;

namespace WebApp.Dtos.Dossier
{
    public class DossierDto
    {
        public Patient? Patient { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; } 
        public DiagnoseCode? DiagnoseCode { get; set; }
        [Required]
        [Display(Name = "Diagnose")]
        public int DiagnoseCodeId { get; set; }
        public bool IsStudent { get; set; }
        public User? IntakeBy  { get; set; }
        public User? SupervisedBy  { get; set; }
        public User?  HeadPractitioner { get; set; }
        
        [Required]
        public int TreatmentPlanId { get; set; }
        [Required]
        [Display(Name = "Intake Doktor")]
        public int IntakeById { get; set; }
        [Required]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }
        
        [Display(Name = "Hoofd Behandelaar")]
        public int HeadPracticionerId { get; set; }
        
        [Display(Name = "Supervisor")]
        public int? SupervisedById { get; set; }
        [Display(Name = "Intake datum")]
        public DateTime AdmissionDate  { get; set; }
        [Required]
        [Display(Name = "Straat")]
        public string Street { get; set; }
        [Required]
        [Display(Name = "Stad")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Post Code")]
        [RegularExpression(@"/^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$/i;", ErrorMessage = "Geen Valide Postcode")]
        public string PostalCode { get; set; }
        [Required]
        [Display(Name = "Huisnummer en toevoeging")]
        public string HouseNumber { get; set; }

        public Core.Domain.Models.TreatmentPlan? TreatmentPlan { get; set; }
        // public IEnumerable<Comments>? Comments  { get; set; }
        public IEnumerable<Core.Domain.Models.Treatment>? Treatments { get; set; }

    }
}