using AutoMapper;
using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Domain;
using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ISWBlacklist.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<UpdateUserResponseDto>> UpdateUserAsync(string userId, UpdateUserDto updateUserDto)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(userId);

                if (existingUser == null)
                {
                    return ApiResponse<UpdateUserResponseDto>.Failed(false, "User not found", 404, null);
                }

                _mapper.Map(updateUserDto, existingUser);

                var result = await _userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    var updatedUserDto = _mapper.Map<UpdateUserResponseDto>(existingUser);
                    return ApiResponse<UpdateUserResponseDto>.Success(updatedUserDto, "User updated successfully", 200);
                }
                else
                {
                    return ApiResponse<UpdateUserResponseDto>.Failed(false, "Failed to update user", 400, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating user: {ex.Message}");
                return ApiResponse<UpdateUserResponseDto>.Failed(false, "An error occurred while updating user", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.Failed(false, "User not found", 400, new List<string>());
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _unitOfWork.UserRepository.DeleteAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                    return ApiResponse<bool>.Success(true, "User deleted successfully", 200);
                }
                else
                {
                    var errors = result.Errors.Select(error => error.Description).ToList();
                    return ApiResponse<bool>.Failed(false, "Failed to delete user", 400, errors);
                }
            }
        }

    }
}
