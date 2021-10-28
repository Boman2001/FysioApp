using System;
using System.Collections.Generic;
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
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string HouseNumber { get; set; }
        
        public virtual  IEnumerable<Dossier> Dossiers { get; set; }
    }
}