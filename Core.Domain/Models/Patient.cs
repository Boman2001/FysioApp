using System;
using Core.Domain.Enums;

namespace Core.Domain.Models
{
    public class Patient : User
    {
        public string PatientNumber { get; set; }
        public string PictureUrl { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
    }
}