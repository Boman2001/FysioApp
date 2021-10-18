using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Models
{
    public class TreatmentPlan : Entity
    {
        public int TreatmentsPerWeek { get; set; }
        public float TimePerSession { get; set; }
        [NotMapped]
        public TreatmentCode TreatmentCode { get; set; }
        public int TreatmentCodeId { get; set; }
        public virtual Treatment Treatment { get; set; }
        
    }
}