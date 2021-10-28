using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Dtos.Auth
{
    public class StaffDto : RegisterDto
    {
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public TimeSpan start { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public TimeSpan end { get; set; }
    }
}