using Azure;
using ISWBlacklist.Application.DTOs.Auth;
using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISWBlacklist.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationService;

        public AuthController(IAuthenticationServices authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("create-user-admin")]
        public async Task<IActionResult> CreateUserAdmin([FromBody] RegisterUserDto createUserAdminDto)
        {
            var response = await _authenticationService.RegisterUserAsync(createUserAdminDto);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppUserLoginDto loginDTO)
        {
            var response = await _authenticationService.LoginAsync(loginDTO);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            var response = await _authenticationService.ValidateTokenAsync(token);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token, string newPassword)
        {
            var response = await _authenticationService.ResetPasswordAsync(email, token, newPassword);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(AppUser user, string currentPassword, string newPassword)
        {
            var response = await _authenticationService.ChangePasswordAsync(user, currentPassword, newPassword);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword(string email, string newPassword, string confirmPassword)
        {
            var response = await _authenticationService.SetPasswordAsync(email, newPassword, confirmPassword);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("email-exists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var exists = await _authenticationService.DoesEmailExistAsync(email);
            if (exists)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await _authenticationService.LogoutAsync();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
