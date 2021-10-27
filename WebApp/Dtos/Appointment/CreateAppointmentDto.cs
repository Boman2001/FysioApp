using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Dtos.Appointment
{
    public class CreateAppointmentDto : AppointmentDto
    {
        public List<SelectListItem> Staff { get; set; }
    }
}