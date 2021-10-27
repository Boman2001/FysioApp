using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Models
{
    public class Dossier : Entity
    {
        public virtual  Patient Patient { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public DiagnoseCode DiagnoseCode { get; set; }
        public int DiagnoseCodeId { get; set; }
        public bool IsStudent { get; set; }
        public virtual  User IntakeBy  { get; set; }
        public virtual  User? SupervisedBy  { get; set; }
        public virtual  User  HeadPractitioner { get; set; }
        public DateTime RegistrationDate  { get; set; }
        public DateTime DismissionDate  { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Housenumber { get; set; }
        public virtual TreatmentPlan TreatmentPlan { get; set; }
        public virtual IEnumerable<Comment> Comments  { get; set; }
        public virtual  IEnumerable<Treatment> Treatments { get; set; }
        
    }


}