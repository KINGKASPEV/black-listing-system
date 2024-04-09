using AutoMapper;
using ISWBlacklist.Application.DTOs.BlacklistedItem;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Common;
using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Domain;
using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ISWBlacklist.Application.Services.Implementations
{
    public class BlacklistService : IBlacklistService
    {
        private readonly ILogger<BlacklistService> _logger;
        private readonly IGenericRepository<Item> _itemRepository;
        private readonly IMapper _mapper;

        public BlacklistService(ILogger<BlacklistService> logger,
                                IGenericRepository<Item> itemRepository,
                                IMapper mapper)
        {
            _logger = logger;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<BlacklistedItemResponseDto>> BlacklistItemAsync(string itemId, string reason)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(itemId);
                if (item == null)
                    return ApiResponse<BlacklistedItemResponseDto>.Failed(false, "Item not found.", StatusCodes.Status404NotFound, new List<string>());

                if (item.IsBlacklisted)
                    return ApiResponse<BlacklistedItemResponseDto>.Failed(false, "Item is already blacklisted.", StatusCodes.Status400BadRequest, new List<string>());

                item.IsBlacklisted = true;
                item.Reason = reason;
                item.BlacklistedAt = DateTime.Now;

                _itemRepository.Update(item);
                await _itemRepository.SaveChangesAsync();

                var responseDto = _mapper.Map<BlacklistedItemResponseDto>(item);
                return ApiResponse<BlacklistedItemResponseDto>.Success(responseDto, "Item blacklisted successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while blacklisting item: {ex.Message}");
                return ApiResponse<BlacklistedItemResponseDto>.Failed(false, "An error occurred while blacklisting item", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<BlacklistedItemResponseDto>> GetBlacklistedItemByIdAsync(string itemId)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(itemId);
                if (item == null)
                    return ApiResponse<BlacklistedItemResponseDto>.Failed(false, "Item not found.", StatusCodes.Status404NotFound, new List<string>());

                var responseDto = _mapper.Map<BlacklistedItemResponseDto>(item);
                return ApiResponse<BlacklistedItemResponseDto>.Success(responseDto, "Blacklisted item found", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting blacklisted item: {ex.Message}");
                return ApiResponse<BlacklistedItemResponseDto>.Failed(false, "An error occurred while getting blacklisted item", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<PageResult<IEnumerable<BlacklistedItemResponseDto>>>> GetBlacklistedItemsAsync(int page, int perPage)
        {
            try
            {
                var blacklistedItems = await _itemRepository.FindAsync(item => item.IsBlacklisted);

                var blacklistedItemDtos = _mapper.Map<IEnumerable<BlacklistedItemResponseDto>>(blacklistedItems);

                var pagedResult = await Pagination<BlacklistedItemResponseDto>.GetPager(blacklistedItemDtos, perPage, page,
                    item => item.Reason, item => item.Id);

                return ApiResponse<PageResult<IEnumerable<BlacklistedItemResponseDto>>>.Success(pagedResult, "Blacklisted items retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting blacklisted items: {ex.Message}");
                return ApiResponse<PageResult<IEnumerable<BlacklistedItemResponseDto>>>.Failed(false, "An error occurred while getting blacklisted items", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }


        public async Task<ApiResponse<string>> RemoveBlacklistedItemAsync(string itemId, string removalReason)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(itemId);
                if (item == null)
                    return ApiResponse<string>.Failed(false, "Item not found.", StatusCodes.Status404NotFound, new List<string>());

                if (!item.IsBlacklisted)
                    return ApiResponse<string>.Failed(false, "Item is not blacklisted.", StatusCodes.Status400BadRequest, new List<string>());

                item.IsBlacklisted = false;
                item.RemovalReason = removalReason;
                item.RemovedAt = DateTime.Now;

                _itemRepository.Update(item);
                await _itemRepository.SaveChangesAsync();

                return ApiResponse<string>.Success("Item removed from the blacklist successfully", "Item removed from the blacklist successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while removing blacklisted item: {ex.Message}");
                return ApiResponse<string>.Failed(false, "An error occurred while removing blacklisted item", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> UpdateBlacklistedItemAsync(string itemId, string reason)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(itemId);
                if (item == null)
                    return ApiResponse<string>.Failed(false, "Item not found.", StatusCodes.Status404NotFound, new List<string>());

                if (!item.IsBlacklisted)
                    return ApiResponse<string>.Failed(false, "Item is not blacklisted.", StatusCodes.Status400BadRequest, new List<string>());

                item.Reason = reason;

                _itemRepository.Update(item);
                await _itemRepository.SaveChangesAsync();

                return ApiResponse<string>.Success("Reason for blacklisting item updated successfully", "Reason for blacklisting item updated successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating reason for blacklisting item: {ex.Message}");
                return ApiResponse<string>.Failed(false, "An error occurred while updating reason for blacklisting item", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }
    }
}
