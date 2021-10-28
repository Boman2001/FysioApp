
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain.Models;

namespace WebApp.Dtos.Dossier
{
    public class DossierDto
    {
        public string Description { get; set; }

        [Required]
        [Display(Name = "Diagnose")]
        public int DiagnoseCodeId { get; set; }

        public bool IsStudent { get; set; }

        [Required] public int TreatmentPlanId { get; set; }

        [Required]
        [Display(Name = "Intake Doktor")]
        public int IntakeById { get; set; }

        [Required] [Display(Name = "Patient")] public int PatientId { get; set; }

        [Display(Name = "Hoofd Behandelaar")] public int HeadPracticionerId { get; set; }

        [Display(Name = "Supervisor")] public int? SupervisedById { get; set; }
        [Display(Name = "Intake datum")] public DateTime AdmissionDate { get; set; }

        #nullable enable
        public DiagnoseCode? DiagnoseCode { get; set; }

        
        public Core.Domain.Models.Patient? Patient { get; set; }
        public int? Age { get; set; }
        public User? IntakeBy { get; set; }
        public User? SupervisedBy { get; set; }
        public User? HeadPractitioner { get; set; }
        
        public Core.Domain.Models.TreatmentPlan? TreatmentPlan { get; set; }
        public IEnumerable<Core.Domain.Models.Treatment>? Treatments { get; set; }
    }
}