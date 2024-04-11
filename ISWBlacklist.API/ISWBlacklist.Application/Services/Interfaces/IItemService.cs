using ISWBlacklist.Application.DTOs.Item;
using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Domain;

namespace ISWBlacklist.Application.Services.Interfaces
{
    public interface IItemService
    {
        Task<ApiResponse<ItemResponseDto>> AddItemAsync(ItemCreationDto creationDto);
        Task<ApiResponse<ItemResponseDto>> GetItemByIdAsync(string itemId);
        Task<ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>> GetAllItemsAsync(int page, int perPage);
        Task<ApiResponse<string>> UpdateItemAsync(string itemId, ItemUpdateDto updateDto);
        Task<ApiResponse<string>> DeleteItemAsync(string itemId);
    }
}
