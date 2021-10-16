namespace Core.Domain.Models
{
    public abstract class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Preposition { get; set; }
        public string Email { get; set; }
        
    }
}