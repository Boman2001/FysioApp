using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Dtos.Dossier
{
    public class CreateDossierDto : DossierDto
    {

        public List<SelectListItem> Staff { get; set; }
        public List<SelectListItem> Patients { get; set; }
        public List<SelectListItem> Diagnoses { get; set; }
        public List<SelectListItem> Treatments { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "There must be more then one treatment per week")]
        public int TreatmentsPerWeek;

        [Required]
        [MinLength(1, ErrorMessage = "a treatment must atleast last 1 minute")]
        public int TimePerSessionInMinutes;

        public TreatmentCode? TreatmentCode;

        [Required] public int TreatmentCodeId;
        
        
    }
}