using System.Collections.Generic;
using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Dtos.Appointment
{
    public class CreateAppointmentDto : AppointmentDto
    {
        public List<SelectListItem> Staff { get; set; }
        public Core.Domain.Models.Dossier Dossier { get; set; }
        public Core.Domain.Models.TreatmentPlan TreatmentPlan { get; set; }
    }
}