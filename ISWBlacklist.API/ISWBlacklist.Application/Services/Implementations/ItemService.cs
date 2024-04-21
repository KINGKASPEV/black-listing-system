using AutoMapper;
using ISWBlacklist.Application.DTOs.Cloudinary;
using ISWBlacklist.Application.DTOs.Item;
using ISWBlacklist.Application.DTOs.User;
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
        private readonly ICloudinaryServices<Item> _cloudinaryServices;

        public ItemService(IGenericRepository<Item> itemRepository, ILogger<ItemService> logger, IMapper mapper, ICloudinaryServices<Item> cloudinaryServices)
        {
            _itemRepository = itemRepository;
            _logger = logger;
            _mapper = mapper;
            _cloudinaryServices = cloudinaryServices;
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

        public async Task<ApiResponse<IEnumerable<ItemResponseDto>>> GetNonBlacklistedItemsAsync()
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();

                var nonBlacklistedItems = items.Where(item => !item.IsBlacklisted);

                var itemDtos = nonBlacklistedItems.Select(item => _mapper.Map<ItemResponseDto>(item)).ToList();

                return ApiResponse<IEnumerable<ItemResponseDto>>.Success(itemDtos, "Non-blacklisted items retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving non-blacklisted items: {ex.Message}");
                return ApiResponse<IEnumerable<ItemResponseDto>>.Failed(false, "An error occurred while retrieving non-blacklisted items", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
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

        public async Task<CloudinaryUploadResponse> UpdatePhoto(UpdatePhotoDTO model)
        {
            try
            {
                var cloudinaryResponse = await _cloudinaryServices.UploadImage(model.PhotoFile);

                if (cloudinaryResponse == null)
                {
                    _logger.LogError("Cloudinary upload failed.");
                    return new CloudinaryUploadResponse { Url = "Default URL or null" };
                }

                var response = new CloudinaryUploadResponse
                {
                    Url = cloudinaryResponse.Url
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating item photo.");
                throw;
            }
        }

        //public async Task<CloudinaryUploadResponse> UpdateItemPhotoByUserId(string id, UpdatePhotoDTO model)
        //{
        //    try
        //    {
        //        var item = await _itemRepository.GetByIdAsync(id);

        //        if (item == null)
        //            throw new Exception("Item not found");

        //        var file = model.PhotoFile;

        //        if (file == null || file.Length <= 0)
        //            throw new Exception("Invalid file size");

        //        var cloudinaryResponse = await _cloudinaryServices.UploadImage(id, file);

        //        if (cloudinaryResponse == null)
        //        {
        //            _logger.LogError($"Failed to upload image for user with ID {id}.");
        //            throw new Exception("Failed to upload image to Cloudinary");
        //        }

        //        item.ImageUrl = cloudinaryResponse.Url;

        //        _itemRepository.Update(item);

        //        await _itemRepository.SaveChangesAsync();

        //        return cloudinaryResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating item photo.");
        //        throw;
        //    }
        //}

        public async Task<ApiResponse<IEnumerable<string>>> GetCategoriesAsync()
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();
                var categories = items
                    .GroupBy(item => item.Category)
                    .Select(group => group.Key)
                    .ToList();

                return ApiResponse<IEnumerable<string>>.Success(categories, "Categories retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving categories: {ex.Message}");
                return ApiResponse<IEnumerable<string>>.Failed(false, "An error occurred while retrieving categories", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }
    }
}
