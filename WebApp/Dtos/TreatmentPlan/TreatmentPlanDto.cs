using System.ComponentModel.DataAnnotations;
using Core.Domain.Models;
using WebApp.Dtos.Dossier;

namespace WebApp.Dtos.TreatmentPlan
{
    public class TreatmentPlanDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "There must be more then one treatment per week")]
        public int TreatmentsPerWeek;

        [Required]
        [MinLength(1, ErrorMessage = "a treatment must atleast last 1 minute")]
        public int TimePerSessionInMinutes;

    }
}