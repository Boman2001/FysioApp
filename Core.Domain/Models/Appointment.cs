using System;
using Core.Domain.Enums;

namespace Core.Domain.Models
{
    public class Appointment : Entity
    {
        public virtual Dossier Dossier { get; set; }

        public DateTime TreatmentDate { get; set; }
        public DateTime TreatmentEndDate { get; set; }
        public RoomType Room { get; set; }
        public virtual User ExcecutedBy { get; set; }
    }
}