using System.Collections.Generic;
using WebApp.Dtos.Treatment;

namespace WebApp.Dtos.Appointment
{
    public class AppointmentIndexDto
    {
        public IEnumerable<AppointmentViewDto> AppointmentViewDtos { get; set; }
        public IEnumerable<TreatmentViewDto> treatmentViewDtos { get; set; }
    }
}