namespace Core.Domain.Models
{
    public class Comment : Entity
    {
        public string CommentBody { get; set; }
        public virtual User CreatedBy { get; set; }
        public bool IsVisiblePatient { get; set; }
        public virtual Dossier isPostedOn { get; set; }
    }
}