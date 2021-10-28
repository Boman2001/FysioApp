using System;
using Core.Domain.Models;

namespace ApplicationServices.ExtensionMethods
{
    public static class StaffExtensions
    {
         public static bool InBuisinessHours(this  Staff staff ,DateTime toCheck)
                {
                    TimeSpan start = staff.start;
                    TimeSpan end = staff.end;
                    TimeSpan checkTimespan =  toCheck.TimeOfDay;
        
                    return (checkTimespan < end && checkTimespan > start && toCheck.DayOfWeek != DayOfWeek.Saturday &&
                            toCheck.DayOfWeek != DayOfWeek.Sunday);
                }
    }
}