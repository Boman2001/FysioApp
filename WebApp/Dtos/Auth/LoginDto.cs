using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.Dtos.Auth
{
    public class LoginDto
    {
        [NotNull]         [Required][EmailAddress] public string Email { get; set; }
        [NotNull]        [Required] [PasswordPropertyText] public string Password { get; set; }
    }
}