using WebApp.Dtos.Dossier;

namespace WebApp.Dtos.Treatment
{
    public class CreateTreatmentDto : TreatmentDto
    {
        public DossierDto DossierDto { get; set; }
    }
}