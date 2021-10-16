namespace Core.Domain.Models
{
    public class DiagnoseCode : Entity
    {
        public int Code { get; set; }
        public string LocationBody { get; set; }
        public string Pathology { get; set; }
    }
}