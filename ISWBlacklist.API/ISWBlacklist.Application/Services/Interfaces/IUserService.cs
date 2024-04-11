﻿using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Domain;

namespace ISWBlacklist.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponseDto>> CreateUserAsync(string userAdminId, CreateUserDto createUserDto);
        Task<ApiResponse<UpdateUserResponseDto>> UpdateUserAsync(string userId, UpdateUserDto updateUserDto);
        Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(string userId);
        Task<ApiResponse<PageResult<IEnumerable<UserResponseDto>>>> GetAllUsersAsync(int perPage, int page);
        Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
        Task<ApiResponse<bool>> DeleteUserAsync(string id);
    }
}
