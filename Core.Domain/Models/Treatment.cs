using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Models
{
    public class Treatment : Entity
    {
        public virtual Dossier Dossier { get; set; }
        public DateTime TreatmentDate { get; set; }
        [NotMapped]
        public TreatmentCode TreatmentCode { get; set; }
        public int TreatmentCodeId { get; set; }
        public string Description  { get; set; }
        public string Particulatities { get; set; }
        public Room Room { get; set; }
        public User ExcecutedBy { get; set; }
        public DateTime ExcecutedOn { get; set; }
       
    }
}