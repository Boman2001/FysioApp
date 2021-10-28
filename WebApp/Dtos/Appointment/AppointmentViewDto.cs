using Core.Domain.Models;

namespace WebApp.Dtos.Appointment
{
    public class AppointmentViewDto : AppointmentDto
    {
        public int Id { get; set; }
        public Staff Practicioner { get; set; }
        public Core.Domain.Models.Patient Patient { get; set; }
    }
}