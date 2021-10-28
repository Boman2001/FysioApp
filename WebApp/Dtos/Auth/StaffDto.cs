using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Dtos.Auth
{
    public class StaffDto : RegisterDto
    {
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public TimeSpan start { get; set; } = new TimeSpan(9, 0, 0);

        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public TimeSpan end { get; set; } = new TimeSpan(17, 0, 0);
    }
}