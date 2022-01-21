using System;
using Core.Domain.Models;

namespace WebApp.Dtos.Appointment
{
    public class AppointmentViewDto : AppointmentDto
    {
        public Staff Practicioner { get; set; }
        public Core.Domain.Models.Patient Patient { get; set; }
        public DateTime TreatmentEndDate { get; set; }
    }
}