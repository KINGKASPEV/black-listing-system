using ISWBlacklist.Application.DTOs.Auth;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISWBlacklist.Application.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IJwtTokenGeneratorService _tokenGeneratorService;

        public AuthenticationService(UserManager<AppUser> userManager, ILogger<AuthenticationService> logger, IGenericRepository<AppUser> userRepository, IJwtTokenGeneratorService tokenGeneratorService)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenGeneratorService = tokenGeneratorService;
        }

        public async Task<Result> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return new Result { Success = false, Message = "User not found" };

                var signInResult = await _userManager.CheckPasswordAsync(user, password);
                if (!signInResult)
                    return new Result { Success = false, Message = "Invalid password" };

                var token = _tokenGeneratorService.GenerateToken(new AppUser { Email = user.Email });
                return new Result { Success = true, Token = token };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
                return new Result { Success = false, Message = "An error occurred during login. Please try again later." };
            }
        }

        public async Task<Result> SetPasswordAsync(string email, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                    return new Result { Success = false, Message = "Password and confirm password do not match" };

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return new Result { Success = false, Message = "User not found" };

                var setPasswordResult = await _userManager.AddPasswordAsync(user, password);
                if (!setPasswordResult.Succeeded)
                    return new Result { Success = false, Message = "Failed to set password" };

                return new Result { Success = true, Message = "Password set successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting password.");
                return new Result { Success = false, Message = "An error occurred while setting password. Please try again later." };
            }
        }
    }
}
