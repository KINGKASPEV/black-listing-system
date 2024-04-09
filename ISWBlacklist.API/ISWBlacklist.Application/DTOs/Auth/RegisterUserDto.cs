using System.ComponentModel.DataAnnotations;

namespace ISWBlacklist.Application.DTOs.Auth
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
