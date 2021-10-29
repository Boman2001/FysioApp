using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain.Enums;

namespace WebApp.Dtos.Appointment
{
    public class AppointmentDto
    {
        
        [Required]
        public DateTime TreatmentDate { get; set; }
        
        [Required]
        public int PracticionerId { get; set; }
        
        [Required]
        public RoomType Room { get; set; }
        [Required]
        public int DossierId { get; set; }
        public int Id { get; set; }
    }
}