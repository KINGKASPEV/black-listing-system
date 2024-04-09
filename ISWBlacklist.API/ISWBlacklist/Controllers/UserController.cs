using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISWBlacklist.Controllers
{
    [Route("api/user")]
    [Authorize(Roles = "UserAdmin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string userAdminId, [FromBody] CreateUserDto createUserDto)
        {
            var response = await _userService.CreateUserAsync(userAdminId, createUserDto);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto updateUserDto)
        {
            var response = await _userService.UpdateUserAsync(userId, updateUserDto);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var response = await _userService.DeleteUserAsync(userId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int perPage, int page)
        {
            var response = await _userService.GetAllUsersAsync(perPage, page);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
