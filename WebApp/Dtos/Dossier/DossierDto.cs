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
        [Display(Name = "Intake Doctor")]
        public int IntakeById { get; set; }
        [Required]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }
        
        [Display(Name = "Head Practitioner")]
        public int HeadPracticionerId { get; set; }
        
        [Display(Name = "Supervisor")]
        public int? SupervisedById { get; set; }
        public DateTime AdmissionDate  { get; set; }
        public Core.Domain.Models.TreatmentPlan? TreatmentPlan { get; set; }
        public IEnumerable<Comment>? Comments  { get; set; }
        public IEnumerable<Core.Domain.Models.Treatment>? Treatments { get; set; }

    }
}