using Microsoft.AspNetCore.Http;

namespace ISWBlacklist.Application.DTOs.User
{
    public class UpdatePhotoDTO
    {
        public IFormFile PhotoFile { get; set; }
    }
}
