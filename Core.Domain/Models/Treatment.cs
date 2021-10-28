using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Enums;

namespace Core.Domain.Models
{
    public class Treatment : Appointment
    {
        [NotMapped]
        public virtual  TreatmentCode TreatmentCode { get; set; }
        public int TreatmentCodeId { get; set; }
        public string Description  { get; set; }
        public string Particulatities { get; set; }

    }
}