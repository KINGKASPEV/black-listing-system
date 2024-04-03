using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Domain;

namespace ISWBlacklist.Application.Services.Interfaces
{
    public interface IUserService
    {
       Task<ApiResponse<UpdateUserResponseDto>> UpdateUserAsync(string userId, UpdateUserDto updateUserDto);
       Task<ApiResponse<bool>> DeleteUserAsync(string id);
    }
}
