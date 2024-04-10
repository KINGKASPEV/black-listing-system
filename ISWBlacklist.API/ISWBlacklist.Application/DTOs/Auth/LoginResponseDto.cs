using ISWBlacklist.Application.DTOs.User;

namespace ISWBlacklist.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public string Email  { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserRole { get; set; }
        public string JWToken { get; set; }
    }
}
