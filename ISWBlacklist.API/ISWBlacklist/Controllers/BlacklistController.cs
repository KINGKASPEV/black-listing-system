using ISWBlacklist.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISWBlacklist.Controllers
{
    [Route("api/blacklist")]
    [Authorize(Roles = "BlacklistAdmin")]
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        private readonly IBlacklistService _blacklistService;

        public BlacklistController(IBlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBlacklistedItem(string itemId, [FromBody] string reason)
        {
            var response = await _blacklistService.BlacklistItemAsync(itemId, reason);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetBlacklistedItemById(string id)
        {
            var response = await _blacklistService.GetBlacklistedItemByIdAsync(id);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlacklistedItems([FromQuery] int page, [FromQuery] int perPage)
        {
            var response = await _blacklistService.GetBlacklistedItemsAsync(page, perPage);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBlacklistedItem([FromRoute] string blacklistedItemId, [FromBody] string removalReason)
        {
            var response = await _blacklistService.RemoveBlacklistedItemAsync(blacklistedItemId, removalReason);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBlacklistedItem([FromRoute] string blacklistedItemId, [FromBody] string reason)
        {
            var response = await _blacklistService.UpdateBlacklistedItemAsync(blacklistedItemId, reason);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
