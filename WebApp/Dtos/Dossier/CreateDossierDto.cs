using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Dtos.Dossier
{
    public class CreateDossierDto : DossierDto
    {
        [Required]
        [Display(Name = "Diagnose")]
        public int DiagnoseCodeId { get; set; }
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
        public List<SelectListItem> Staff { get; set; }
        public List<SelectListItem> Patients { get; set; }
        public List<SelectListItem> Diagnoses { get; set; }
    }
}