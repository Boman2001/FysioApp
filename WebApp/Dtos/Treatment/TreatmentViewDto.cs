using Core.Domain.Models;

namespace WebApp.Dtos.Treatment
{
    public class TreatmentViewDto : TreatmentDto
    {
        public User Practicioner { get; set; }
    }
}