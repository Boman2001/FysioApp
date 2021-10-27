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
        public DateTime TreatmentDate { get; set; }
        [Required]
        public int TreatmentCodeId { get; set; }
        public string Description  { get; set; }
        
        [Required]
        public int PracticionerId { get; set; }
        
        [Required]
        public RoomType Room { get; set; }

        
        public TreatmentCode? TreatmentCode { get; set; }
    }
}