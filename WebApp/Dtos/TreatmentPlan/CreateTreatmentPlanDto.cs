using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Dtos.Dossier;

namespace WebApp.Dtos.TreatmentPlan
{
    public class CreateTreatmentPlanDto : TreatmentPlanDto {
        public CreateTreatmentPlanDto()
        {
            Treatments = new List<SelectListItem>();
        }

        public List<SelectListItem> Treatments { get; set; }
        public int DossierId { get; set; }
    }
}