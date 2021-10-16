namespace Core.Domain.Models
{
    public class TreatmentPlan : Entity
    {
        public int TreatmentsPerWeek { get; set; }
        public float TimePerSession { get; set; }
        public TreatmentCode TreatmentCode { get; set; }
    }
}