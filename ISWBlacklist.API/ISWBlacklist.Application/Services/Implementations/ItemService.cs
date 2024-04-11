using AutoMapper;
using ISWBlacklist.Application.DTOs.Item;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Common;
using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Domain;
using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ISWBlacklist.Application.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IGenericRepository<Item> _itemRepository;
        private readonly ILogger<ItemService> _logger;
        private readonly IMapper _mapper;

        public ItemService(IGenericRepository<Item> itemRepository, ILogger<ItemService> logger, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ItemResponseDto>> AddItemAsync(ItemCreationDto creationDto)
        {
            try
            {
                var item = _mapper.Map<Item>(creationDto);
                item.IsBlacklisted = false;

                await _itemRepository.AddAsync(item);
                await _itemRepository.SaveChangesAsync();

                var responseDto = _mapper.Map<ItemResponseDto>(item);
                return ApiResponse<ItemResponseDto>.Success(responseDto, "Item added successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding item: {ex.Message}");
                return ApiResponse<ItemResponseDto>.Failed(false, "An error occurred while adding item", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<ItemResponseDto>> GetItemByIdAsync(string itemId)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(itemId);
                if (item == null)
                    return ApiResponse<ItemResponseDto>.Failed(false, "Item not found.", StatusCodes.Status404NotFound, new List<string>());

                var responseDto = _mapper.Map<ItemResponseDto>(item);
                return ApiResponse<ItemResponseDto>.Success(responseDto, "Item retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving item: {ex.Message}");
                return ApiResponse<ItemResponseDto>.Failed(false, "An error occurred while retrieving item", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>> GetAllItemsAsync(int page, int perPage)
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();

                var itemDtos = new List<ItemResponseDto>();
                foreach (var item in items)
                {
                    var itemDto = _mapper.Map<ItemResponseDto>(item);
                    itemDtos.Add(itemDto);
                }

                var paginatedItems = await Pagination<ItemResponseDto>.GetPager(itemDtos, perPage, page,
                    item => item.Name, item => item.Id);

                return ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>.Success(paginatedItems, "Items retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving items: {ex.Message}");
                return ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>.Failed(false, "An error occurred while retrieving items", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<ItemResponseDto>>> GetAllItemsAsync()
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();

                var itemDtos = items.Select(item => _mapper.Map<ItemResponseDto>(item)).ToList();

                return ApiResponse<IEnumerable<ItemResponseDto>>.Success(itemDtos, "Items retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving items: {ex.Message}");
                return ApiResponse<IEnumerable<ItemResponseDto>>.Failed(false, "An error occurred while retrieving items", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }


        public async Task<ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>> GetNonBlacklistedItemsAsync(int page, int perPage)
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();

                var nonBlacklistedItems = items.Where(item => !item.IsBlacklisted);

                var itemDtos = nonBlacklistedItems.Select(item => _mapper.Map<ItemResponseDto>(item)).ToList();

                var paginatedItems = await Pagination<ItemResponseDto>.GetPager(itemDtos, perPage, page,
                    item => item.Name, item => item.Id);

                return ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>.Success(paginatedItems, "Non-blacklisted items retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving non-blacklisted items: {ex.Message}");
                return ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>.Failed(false, "An error occurred while retrieving non-blacklisted items", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> UpdateItemAsync(string itemId, ItemUpdateDto updateDto)
        {
            try
            {
                var existingItem = await _itemRepository.GetByIdAsync(itemId);
                if (existingItem == null)
                    return ApiResponse<string>.Failed(false, "Item not found.", StatusCodes.Status404NotFound, new List<string>());

                _mapper.Map(updateDto, existingItem);
                existingItem.ModifiedAt = DateTime.Now;

                _itemRepository.Update(existingItem);
                await _itemRepository.SaveChangesAsync();

                return ApiResponse<string>.Success("Item updated successfully", "Item updated successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating item: {ex.Message}");
                return ApiResponse<string>.Failed(false, "An error occurred while updating item", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> DeleteItemAsync(string itemId)
        {
            try
            {
                var existingItem = await _itemRepository.GetByIdAsync(itemId);
                if (existingItem == null)
                    return ApiResponse<string>.Failed(false, "Item not found.", StatusCodes.Status404NotFound, new List<string>());

                _itemRepository.DeleteAsync(existingItem);
                await _itemRepository.SaveChangesAsync();

                return ApiResponse<string>.Success("Item deleted successfully", "Item deleted successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting item: {ex.Message}");
                return ApiResponse<string>.Failed(false, "An error occurred while deleting item", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }
    }
}
