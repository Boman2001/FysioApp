namespace Core.Domain.Models
{
    public class TreatmentCode : Entity
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public bool ExplanationRequired { get; set; }

    }
}