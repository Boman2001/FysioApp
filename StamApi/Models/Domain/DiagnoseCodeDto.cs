namespace StamApi.Models.Domain
{
    public class DiagnoseCodeDto: ApiDto
    {
        public string Code { get; set; }
        public string LocationBody { get; set; }
        public string Pathology { get; set; }
    }
}