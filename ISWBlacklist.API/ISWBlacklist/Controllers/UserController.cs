using Azure;
using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Domain.Entities;
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
        private readonly ICloudinaryServices<AppUser> _cloudinaryServices;

        public UserController(IUserService userService, ICloudinaryServices<AppUser> cloudinaryServices)
        {
            _userService = userService;
            _cloudinaryServices = cloudinaryServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string userAdminId, [FromBody] CreateUserDto createUserDto)
        {
            var response = await _userService.CreateUserAsync(userAdminId, createUserDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto updateUserDto)
        {
            var response = await _userService.UpdateUserAsync(userId, updateUserDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var response = await _userService.GetUserByIdAsync(userId);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int page, int perPage)
        {
            var response = await _userService.GetAllUsersAsync(page, perPage);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var response = await _userService.DeleteUserAsync(userId);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        //[HttpPut("{userId}/photo")]
        //public async Task<IActionResult> UpdateUserPhoto(string userId, IFormFile photo)
        //{
        //    try
        //    {
        //        var updatePhotoDto = new UpdatePhotoDTO { PhotoFile = photo };
        //        var response = await _userService.UpdateUserPhotoByUserId(userId, updatePhotoDto);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating user photo.");
        //    }
        //}
    }
}
