namespace ISWBlacklist.Application.DTOs.User
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateModified { get; set; }
        public UserRole Role { get; set; }
    }
}
