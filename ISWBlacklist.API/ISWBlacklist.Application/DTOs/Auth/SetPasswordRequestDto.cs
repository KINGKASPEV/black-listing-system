namespace ISWBlacklist.Application.DTOs.Auth
{
    public class SetPasswordRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
