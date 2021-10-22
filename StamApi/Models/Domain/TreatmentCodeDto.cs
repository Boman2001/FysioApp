namespace StamApi.Models.Domain
{
    public class TreatmentCodeDto : ApiDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool ExplanationRequired { get; set; }
    }
}