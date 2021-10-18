using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Models
{
    public class Dossier : Entity
    {
        public Patient Patient { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public DiagnoseCode DiagnoseCode { get; set; }
        public int DiagnoseCodeId { get; set; }
        public bool IsByStudent { get; set; }
        public User IntakeBy  { get; set; }
        public User? SupervisedBy  { get; set; }
        public DateTime RegistrationDate  { get; set; }
        public IEnumerable<Comment> Comments  { get; set; }
        public IEnumerable<Treatment> Treatments { get; set; }
        
    }


}