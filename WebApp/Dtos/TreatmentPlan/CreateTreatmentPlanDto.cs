using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Dtos.Dossier;

namespace WebApp.Dtos.TreatmentPlan
{
    public class CreateTreatmentPlanDto : TreatmentPlanDto {
        public int DossierId { get; set; }
    }
}