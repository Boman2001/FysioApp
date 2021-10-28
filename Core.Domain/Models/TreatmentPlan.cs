using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Models
{
    public class TreatmentPlan : Entity
    {
        public int TreatmentsPerWeek { get; set; }
        public int TimePerSessionInMinutes { get; set; }

    }
}