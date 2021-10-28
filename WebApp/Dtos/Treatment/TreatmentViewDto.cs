using Core.Domain.Models;

namespace WebApp.Dtos.Treatment
{
    public class TreatmentViewDto : TreatmentDto
    {
        public int Id { get; set; }
        public User Practicioner { get; set; }
    }
}