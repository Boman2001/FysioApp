using System;

namespace ApplicationServices.Helpers
{
    public class DatetimeHelper : IDatetimeHelper
    {
        public DateTime Now()
        {
            return  DateTime.Now;
        }
    }
}