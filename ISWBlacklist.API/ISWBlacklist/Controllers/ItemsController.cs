using ISWBlacklist.Application.DTOs.Item;
using ISWBlacklist.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISWBlacklist.Controllers
{
    [Route("api/items")]
    [Authorize]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ItemCreationDto creationDto)
        {
            var response = await _itemService.AddItemAsync(creationDto);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItem(string itemId)
        {
            var response = await _itemService.GetItemByIdAsync(itemId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all-items-paginated")]
        public async Task<IActionResult> GetItems([FromQuery] int page, [FromQuery] int perPage)
        {
            var response = await _itemService.GetAllItemsAsync(page, perPage);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all-items")]
        public async Task<IActionResult> GetItems()
        {
            var response = await _itemService.GetAllItemsAsync();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("non-blacklisted-items")]
        public async Task<IActionResult> GetItemsNotBlacklisted([FromQuery] int page, [FromQuery] int perPage)
        {
            var response = await _itemService.GetNonBlacklistedItemsAsync(page, perPage);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem(string itemId, [FromBody] ItemUpdateDto updateDto)
        {
            var response = await _itemService.UpdateItemAsync(itemId, updateDto);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem(string itemId)
        {
            var response = await _itemService.DeleteItemAsync(itemId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}