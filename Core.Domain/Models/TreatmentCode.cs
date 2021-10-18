namespace Core.Domain.Models
{
    public class TreatmentCode : Entity
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool ExplanationRequired { get; set; }

    }
}