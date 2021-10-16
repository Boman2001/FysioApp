using System;

namespace Core.Domain.Models
{
    public class Treatment
    {
        public DateTime TreatmentDate { get; set; }
        public TreatmentCode TreatmentCode { get; set; }
        public string Description  { get; set; }
        public string Particulatities { get; set; }
        public Room Room { get; set; }
        public User ExcecutedBy { get; set; }
        public DateTime ExcecutedOn { get; set; }
    }
}