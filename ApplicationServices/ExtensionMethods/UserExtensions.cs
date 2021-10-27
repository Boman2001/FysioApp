using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Models;

namespace ApplicationServices.ExtensionMethods
{
    public static class UserExtensions
    {
        public static bool IsAvailable(this User user, DateTime start, DateTime end)
        {
            List<Tuple<DateTime, DateTime>> timeSlots = new List<Tuple<DateTime, DateTime>>();
            user.HeadPractisionerOf.ToList().ForEach(dossier =>
            {
                dossier.Treatments.ToList().ForEach(treatment => timeSlots.Add(new Tuple<DateTime, DateTime>(treatment.TreatmentDate, treatment.TreatmentEndDate)) );
            });

            return timeSlots.TrueForAll(timeSlot =>
            {
                return start.Ticks <= timeSlot.Item1.Ticks && start.Ticks >= timeSlot.Item2.Ticks &&
                       end.Ticks <= timeSlot.Item1.Ticks && end.Ticks >= timeSlot.Item2.Ticks;

            });
        }
        
        public static string GetFormattedName(this User user)
        {
            return user.Preposition == null ? $"{user.FirstName} {user.LastName}" : $"{user.FirstName} {user.Preposition} {user.LastName}";
        }
    }
}

