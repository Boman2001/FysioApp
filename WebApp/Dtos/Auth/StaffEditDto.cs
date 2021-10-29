using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Dtos.Auth
{
    public class StaffEditDto : EditDto
    {
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        [Required]
        public TimeSpan start { get; set; } = new TimeSpan(9, 0, 0);

        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        [Required]
        public TimeSpan end { get; set; } = new TimeSpan(17, 0, 0);
    }
}