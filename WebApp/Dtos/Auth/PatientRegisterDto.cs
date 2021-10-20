using System;
using Core.Domain.Enums;

namespace WebApp.Dtos.Auth
{
    public class PatientRegisterDto
    {
        public string PatientNumber { get; set; }
        public string PictureUrl { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
    }
}