using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Enums;

namespace Core.Domain.Models
{
    public class Treatment : Entity
    {
        public virtual Dossier Dossier { get; set; }
        public DateTime TreatmentDate { get; set; }
        public DateTime TreatmentEndDate { get; set; }
        [NotMapped]
        public virtual  TreatmentCode? TreatmentCode { get; set; }
        public int? TreatmentCodeId { get; set; }
        public string Description  { get; set; }
        public string? Particulatities { get; set; }
        public RoomType Room { get; set; }
        public virtual  User ExcecutedBy { get; set; }

    }
}