using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Dtos.Dossier;

namespace WebApp.Dtos.Treatment
{
    public class CreateTreatmentDto : TreatmentDto
    {
        public List<SelectListItem> Treatments { get; set; }
        
        public List<SelectListItem> Staff { get; set; }
        public Core.Domain.Models.Dossier Dossier { get; set; }
        public int DossierId { get; set; }
    }
}