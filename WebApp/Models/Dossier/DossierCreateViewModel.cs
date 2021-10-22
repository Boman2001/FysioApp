using System.Collections.Generic;
using Blazorise;
using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Dossier
{
    public class DossierCreateViewModel
    {
        public List<SelectListItem> Staff { get; set; }
        public List<SelectListItem> Patients { get; set; }
      
    }
}