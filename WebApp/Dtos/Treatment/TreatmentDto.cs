using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Enums;
using Core.Domain.Models;

namespace WebApp.Dtos.Treatment
{
    public class TreatmentDto
    {
        [Required]
        [Display(Name = "Uitgevoerd op")]
        public DateTime TreatmentDate { get; set; }
        [Required]
        [Display(Name = "Behandelingscode")]
        public int TreatmentCodeId { get; set; }
        [Display(Name = "Omschrijving")]
        public string Description  { get; set; }
        
        [Required]
        [Display(Name = "Uitgevoerd Door:")]
        public int PracticionerId { get; set; }
        
        [Required]
        [Display(Name = "Kamertype")]
        public RoomType Room { get; set; }
        [Display(Name = "Bijzonderheden")]
        public string Particulatities { get; set; }
        #nullable enable
        [Display(Name = "Behandelings code")]
        public TreatmentCode? TreatmentCode { get; set; }
    }
}