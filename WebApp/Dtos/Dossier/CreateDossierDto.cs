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
        public List<SelectListItem> TreatmentItems { get; set; }

        [Required]
        [Range(1, 999, ErrorMessage = "There must be more then one treatment per week")]
        public int TreatmentsPerWeek { get; set; }

        [Required]
        [Range(1, 999, ErrorMessage = "a treatment must atleast last 1 minute")]
        public int TimePerSessionInMinutes  { get; set; }

    }
}