using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.Dtos.Auth
{
    public abstract class RegisterDto
    {
        [NotNull] [Required] public string FirstName { get; set; }
        [NotNull] [Required] public string LastName { get; set; }
#nullable enable
        [AllowNull] public string? Preposition { get; set; }
#nullable disable
        [NotNull] [Required] [EmailAddress] public string Email { get; set; }

        [NotNull]
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}