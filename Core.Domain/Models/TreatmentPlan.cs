using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Models
{
    public class TreatmentPlan : Entity
    {
        public int TreatmentsPerWeek { get; set; }
        public int TimePerSessionInMinutes { get; set; }
        [NotMapped]
        public TreatmentCode TreatmentCode { get; set; }
        public int TreatmentCodeId { get; set; }
        [NotMapped]
        public virtual IEnumerable<Treatment> Treatments { get; set; }
        
    }
}