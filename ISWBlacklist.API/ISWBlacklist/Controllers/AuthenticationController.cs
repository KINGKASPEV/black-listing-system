using ISWBlacklist.Application.DTOs.Auth;
using ISWBlacklist.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISWBlacklist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authenticationService.LoginAsync(request.Email, request.Password);
            if (result.Success)
                return Ok(new { token = result.Token });
            else
            {
                _logger.LogInformation(result.Message);
                return Unauthorized(new { message = result.Message });
            }
        }

        [HttpPost("set_password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequestDto request)
        {
            var result = await _authenticationService.SetPasswordAsync(request.Email, request.Password, request.ConfirmPassword);
            if (result.Success)
                return Ok(new { message = result.Message });
            else
            {
                _logger.LogInformation(result.Message);
                return BadRequest(new { message = result.Message });
            }
        }
    }
}
