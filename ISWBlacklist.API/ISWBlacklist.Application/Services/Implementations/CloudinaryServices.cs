using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ISWBlacklist.Application.DTOs.Cloudinary;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;


namespace ISWBlacklist.Application.Services.Implementations
{
    public class CloudinaryServices<TEntity> : ICloudinaryServices<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly Cloudinary _cloudinary;

        public CloudinaryServices(
            IGenericRepository<TEntity> repository,
            IConfiguration configuration)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            var cloudinarySettings = new CloudinarySettings();
            configuration.GetSection("CloudinarySettings").Bind(cloudinarySettings);

            _cloudinary = new Cloudinary(new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret));
        }

        public async Task<CloudinaryUploadResponse> UploadImage(IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            var response = new CloudinaryUploadResponse
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.AbsoluteUri.ToString(),
            };
            return response;
        }

    }
}
