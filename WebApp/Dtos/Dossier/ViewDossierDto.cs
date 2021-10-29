using System.Collections.Generic;
using WebApp.Dtos.Appointment;
using WebApp.Dtos.Comment;
using WebApp.Dtos.Treatment;
using WebApp.Dtos.TreatmentPlan;

namespace WebApp.Dtos.Dossier
{
    public class ViewDossierDto : DossierDto

    {
#nullable enable
        public new TreatmentPlanDto? TreatmentPlan { get; set; }
        public IEnumerable<ViewCommentDto>? Comments { get; set; }
        public new IEnumerable<TreatmentViewDto>? Treatments { get; set; }
        public IEnumerable<AppointmentViewDto>? Appointments { get; set; }
    }
}