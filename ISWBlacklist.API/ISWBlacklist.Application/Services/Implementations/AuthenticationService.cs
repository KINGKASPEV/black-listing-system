using AutoMapper;
using ISWBlacklist.Application.DTOs.Auth;
using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Domain;
using ISWBlacklist.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ISWBlacklist.Application.Services.Implementations
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AuthenticationServices> _logger;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AuthenticationServices(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AuthenticationServices> logger,
            IConfiguration config, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UserResponseDto>> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            try
            {
                var user = _mapper.Map<AppUser>(registerUserDto);
                var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);
                if (existingUser != null)
                    return ApiResponse<UserResponseDto>.Failed(false, "User with the specified email already exists.", 400, null);

                var result = await _userManager.CreateAsync(user, registerUserDto.Password);
                if (!result.Succeeded)
                    return ApiResponse<UserResponseDto>.Failed(false, "Failed to create user.", 500, result.Errors.Select(e => e.Description).ToList());

                if (!await _roleManager.RoleExistsAsync("UserAdmin"))
                    await _roleManager.CreateAsync(new IdentityRole("UserAdmin"));

                await _userManager.AddToRoleAsync(user, "UserAdmin");
                var userResponseDto = _mapper.Map<UserResponseDto>(user);

                return ApiResponse<UserResponseDto>.Success(userResponseDto, "User registered successfully.", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user.");
                return ApiResponse<UserResponseDto>.Failed(false, "An error occurred while registering user.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(AppUserLoginDto loginDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                    return ApiResponse<LoginResponseDto>.Failed(false, "User not found.", StatusCodes.Status404NotFound, new List<string>());

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

                switch (result)
                {
                    case { Succeeded: true }:
                        var jwtSettings = _config.GetSection("JwtSettings");
                        var jwtService = new JwtService(
                            jwtSettings["Secret"],
                            jwtSettings["ValidIssuer"],
                            jwtSettings["ValidAudience"]);

                        var roles = await _userManager.GetRolesAsync(user);
                        var userRoles = await Task.WhenAll(roles.Select(roleName => _roleManager.FindByNameAsync(roleName)));

                        var response = new LoginResponseDto
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            UserRole = userRoles.FirstOrDefault()?.Name,
                            JWToken = jwtService.GenerateToken(user.Id, user.Email, roles.ToArray())
                        };
                        return ApiResponse<LoginResponseDto>.Success(response, "Logged In Successfully", StatusCodes.Status200OK);

                    default:
                        return ApiResponse<LoginResponseDto>.Failed(false, "Login failed. Invalid email or password, go ahead to set your password", StatusCodes.Status401Unauthorized, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some error occurred while logging in." + ex.Message);
                return ApiResponse<LoginResponseDto>.Failed(false, "Some error occurred while logging in." + ex.Message, StatusCodes.Status500InternalServerError, new List<string>() { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:Secret").Value);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config.GetSection("JwtSettings:ValidIssuer").Value,
                    ValidAudience = _config.GetSection("JwtSettings:ValidAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

                return new ApiResponse<string>(true, "Token is valid.", StatusCodes.Status200OK, null, new List<string>());
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "Token validation failed");
                return new ApiResponse<string>(false, "Token validation failed.", StatusCodes.Status400BadRequest, null, new List<string>() { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token validation");
                return new ApiResponse<string>(false, "Error occurred during token validation", StatusCodes.Status500InternalServerError, null, new List<string>() { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return new ApiResponse<string>(false, "User not found.", StatusCodes.Status404NotFound, null, new List<string>());

                if (token.Contains(" "))
                    token = token.Replace(" ", "+");

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded)
                    return new ApiResponse<string>(true, "Password reset successful.", StatusCodes.Status200OK, null, new List<string>());
                return new ApiResponse<string>(false, "Password reset failed.", StatusCodes.Status400BadRequest, null, result.Errors.Select(error => error.Description).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resetting password for user with email {Email}", email);
                return new ApiResponse<string>(true, "Error occurred while resetting password", StatusCodes.Status500InternalServerError, null, new List<string>() { "An unexpected error occurred while resetting the password." });
            }
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            try
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (result.Succeeded)
                    return new ApiResponse<string>(true, "Password changed successfully.", StatusCodes.Status200OK, null, new List<string>());
                return new ApiResponse<string>(false, "Password change failed.", StatusCodes.Status400BadRequest, null, result.Errors.Select(error => error.Description).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password");
                return new ApiResponse<string>(true, "Error occurred while changing password", StatusCodes.Status500InternalServerError, null, new List<string>() { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> SetPasswordAsync(string email, string newPassword, string confirmPassword)
        {
            try
            {
                if (newPassword != confirmPassword)
                    return new ApiResponse<string>(false, "Passwords do not match.", StatusCodes.Status400BadRequest, null, new List<string>());

                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return new ApiResponse<string>(false, $"User with the email address not found.", StatusCodes.Status404NotFound, null, new List<string>());

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded)
                    return new ApiResponse<string>(true, "Password set successfully.", StatusCodes.Status200OK, null, new List<string>());
                return new ApiResponse<string>(false, "Password set failed.", StatusCodes.Status400BadRequest, null, result.Errors.Select(error => error.Description).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting password for user with email {Email}", email);
                return new ApiResponse<string>(true, "Error occurred while setting password", StatusCodes.Status500InternalServerError, null, new List<string>() { "An unexpected error occurred while setting the password." });
            }
        }

        public async Task<bool> DoesEmailExistAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if email exists.");
                return false;
            }
        }

        public async Task<ApiResponse<string>> CheckEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)               
                    return ApiResponse<string>.Failed(false, "Email does not exist", StatusCodes.Status404NotFound, null);
                
                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (hasPassword)
                
                    return ApiResponse<string>.Success("Email exists with password", "Email exists with password", StatusCodes.Status200OK);

                return ApiResponse<string>.Success("Email exists without password", "Email exists without password", StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking email existence and password status.");
                return ApiResponse<string>.Failed(false, "An error occurred while checking email existence and password status.", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new ApiResponse<string>(true, "User logged out successfully.", StatusCodes.Status200OK, null, new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging out");
                return new ApiResponse<string>(true, "Error occurred while logging out", StatusCodes.Status500InternalServerError, null, new List<string>() { ex.Message });
            }
        }
    }
}
