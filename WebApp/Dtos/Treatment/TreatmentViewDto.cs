using System;
using Core.Domain.Models;

namespace WebApp.Dtos.Treatment
{
    public class TreatmentViewDto : TreatmentDto
    {
        public int Id { get; set; }
        public User Practicioner { get; set; }
        public Core.Domain.Models.Patient Patient { get; set; }
        public int DossierId { get; set; }
        public DateTime createdAt { get; set; }
    }
}