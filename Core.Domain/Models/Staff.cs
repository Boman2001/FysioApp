using System;

namespace Core.Domain.Models
{
    public class Staff : User
    {
        public TimeSpan start { get; set; }

        public TimeSpan end { get;set; }
    }
}