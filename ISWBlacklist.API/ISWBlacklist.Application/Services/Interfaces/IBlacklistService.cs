using ISWBlacklist.Application.DTOs.BlacklistedItem;
using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Domain;

namespace ISWBlacklist.Application.Services.Interfaces
{
    public interface IBlacklistService
    {
        Task<ApiResponse<BlacklistedItemResponseDto>> BlacklistItemAsync(string itemId, string reason);
        Task<ApiResponse<BlacklistedItemResponseDto>> GetBlacklistedItemByIdAsync(string itemId);
        Task<ApiResponse<PageResult<IEnumerable<BlacklistedItemResponseDto>>>> GetBlacklistedItemsAsync(int page, int perPage);
        Task<ApiResponse<string>> RemoveBlacklistedItemAsync(string itemId, string removalReason);
        Task<ApiResponse<string>> UpdateBlacklistedItemAsync(string itemId, string reason);
    }
}
