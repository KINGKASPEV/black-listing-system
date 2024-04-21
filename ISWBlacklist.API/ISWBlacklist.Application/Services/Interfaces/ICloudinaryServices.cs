using ISWBlacklist.Application.DTOs.Cloudinary;
using Microsoft.AspNetCore.Http;

namespace ISWBlacklist.Application.Services.Interfaces
{
    public interface ICloudinaryServices<T> where T : class
    {
        Task<CloudinaryUploadResponse> UploadImage(IFormFile file);
        //Task<CloudinaryUploadResponse> UploadImage(string entityId, IFormFile file);
    }
}
