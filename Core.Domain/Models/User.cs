using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Models
{
    public abstract class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Preposition { get; set; }
        public string Email { get; set; }
        [NotMapped]
        public virtual IEnumerable<Dossier> IntakesDone { get; set; }
        [NotMapped]
        public virtual IEnumerable<Dossier> IntakesSupervised { get; set; }
        [NotMapped]
        public virtual IEnumerable<Comment> CommentsCreated { get; set; }
        [NotMapped]
        public virtual IEnumerable<Treatment> TreatmentsDone { get; set; }

        
    }
}