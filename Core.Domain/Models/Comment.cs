namespace Core.Domain.Models
{
    public class Comment : Entity
    {
        public string CommentBody { get; set; }
        public User CreatedBy { get; set; }
        public bool IsVisiblePatient { get; set; }
    }
}