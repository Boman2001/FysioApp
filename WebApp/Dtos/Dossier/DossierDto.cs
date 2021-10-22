using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Models;

namespace WebApp.Dtos.Dossier
{
    public class DossierDto
    {
        public Patient? Patient { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; } 
        public DiagnoseCode? DiagnoseCode { get; set; }
        public bool IsStudent { get; set; }
        public User? IntakeBy  { get; set; }
        public User? SupervisedBy  { get; set; }
        public User?  HeadPractitioner { get; set; }
        public DateTime AdmissionDate  { get; set; }
        public TreatmentPlan? TreatmentPlan { get; set; }
        public IEnumerable<Comment>? Comments  { get; set; }
        public IEnumerable<Treatment>? Treatments { get; set; }

    }
}