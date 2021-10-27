using System.Collections.Generic;
using Core.Domain.Models;
using WebApp.Dtos.Comment;
using WebApp.Dtos.Treatment;
using WebApp.Dtos.TreatmentPlan;

namespace WebApp.Dtos.Dossier
{
    public class ViewDossierDto : DossierDto

    {
        public int Id { get; set; }
        public TreatmentPlanDto? TreatmentPlan { get; set; }
        public IEnumerable<ViewCommentDto>? Comments  { get; set; }
        public IEnumerable<TreatmentViewDto>? Treatments { get; set; }
    }
}