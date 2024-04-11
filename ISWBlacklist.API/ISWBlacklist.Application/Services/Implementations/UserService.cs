using AutoMapper;
using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Common;
using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Domain;
using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ISWBlacklist.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IGenericRepository<AppUser> _userRepository;

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ILogger<UserService> logger, IGenericRepository<AppUser> userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<UserResponseDto>> CreateUserAsync(string userAdminId, CreateUserDto createUserDto)
        {
            try
            {
                var roleName = createUserDto.Role.ToString();
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var errors = new List<string> { $"Role '{roleName}' does not exist." };
                    return ApiResponse<UserResponseDto>.Failed(false, $"Role '{roleName}' does not exist.", StatusCodes.Status400BadRequest, errors);
                }

                var userAdmin = await _userManager.FindByIdAsync(userAdminId);
                if (userAdmin == null)
                    return ApiResponse<UserResponseDto>.Failed(false, "UserAdmin not found.", StatusCodes.Status404NotFound, new List<string>());

                var existingUser = await _userManager.FindByEmailAsync(createUserDto.Email);
                if (existingUser != null)
                    return ApiResponse<UserResponseDto>.Failed(false, "User with this email address already exists.", StatusCodes.Status400BadRequest, new List<string>());

                var newUser = new AppUser { UserName = createUserDto.Email, Email = createUserDto.Email };
                var result = await _userManager.CreateAsync(newUser);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(error => error.Description).ToList();
                    return ApiResponse<UserResponseDto>.Failed(false, "User creation failed.", StatusCodes.Status400BadRequest, errors);
                }

                result = await _userManager.AddToRoleAsync(newUser, roleName);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(error => error.Description).ToList();
                    return ApiResponse<UserResponseDto>.Failed(false, "Failed to assign role to the user.", StatusCodes.Status400BadRequest, errors);
                }

                var userResponse = _mapper.Map<UserResponseDto>(newUser);

                return ApiResponse<UserResponseDto>.Success(userResponse, $"User created successfully with the role: {roleName}", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                return ApiResponse<UserResponseDto>.Failed(false, "An unexpected error occurred while creating user.", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<UpdateUserResponseDto>> UpdateUserAsync(string userId, UpdateUserDto updateUserDto)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(userId);

                if (existingUser == null)
                    return ApiResponse<UpdateUserResponseDto>.Failed(false, "User not found", 404, null);

                _mapper.Map(updateUserDto, existingUser);

                var result = await _userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    var updatedUserDto = _mapper.Map<UpdateUserResponseDto>(existingUser);
                    return ApiResponse<UpdateUserResponseDto>.Success(updatedUserDto, "User updated successfully", 200);
                }
                else
                {
                    return ApiResponse<UpdateUserResponseDto>.Failed(false, "Failed to update user", 400, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating user: {ex.Message}");
                return ApiResponse<UpdateUserResponseDto>.Failed(false, "An error occurred while updating user", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<PageResult<IEnumerable<UserResponseDto>>>> GetAllUsersAsync(int page, int perPage)
        {
            try
            {
                var allUsers = await _userRepository.GetAllAsync();

                var userDtos = new List<UserResponseDto>();
                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userDto = _mapper.Map<UserResponseDto>(user);
                    userDto.Role = roles.FirstOrDefault();
                    userDtos.Add(userDto);
                }

                var paginatedUsers = await Pagination<UserResponseDto>.GetPager(userDtos, perPage, page,
                    user => user.Email, user => user.Id);

                return ApiResponse<PageResult<IEnumerable<UserResponseDto>>>.Success(paginatedUsers, "Users retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving users: {ex.Message}");
                return ApiResponse<PageResult<IEnumerable<UserResponseDto>>>.Failed(false, "An error occurred while retrieving users", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            try
            {
                var allUsers = await _userRepository.GetAllAsync();

                var userDtos = new List<UserResponseDto>();
                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userDto = _mapper.Map<UserResponseDto>(user);
                    userDto.Role = roles.FirstOrDefault();
                    userDtos.Add(userDto);
                }

                return ApiResponse<IEnumerable<UserResponseDto>>.Success(userDtos, "Users retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving users: {ex.Message}");
                return ApiResponse<IEnumerable<UserResponseDto>>.Failed(false, "An error occurred while retrieving users", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }


        //public async Task<ApiResponse<bool>> DeleteUserAsync(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return ApiResponse<bool>.Failed(false, "User not found", 400, new List<string>());
        //    }
        //    else
        //    {
        //        var result = await _userManager.DeleteAsync(user);
        //        if (result.Succeeded)
        //        {
        //            _userRepository.DeleteAsync(user);
        //            await _userRepository.SaveChangesAsync();
        //            return ApiResponse<bool>.Success(true, "User deleted successfully", 200);
        //        }
        //        else
        //        {
        //            var errors = result.Errors.Select(error => error.Description).ToList();
        //            return ApiResponse<bool>.Failed(false, "Failed to delete user", 400, errors);
        //        }
        //    }
        //}

        //public async Task<ApiResponse<bool>> DeleteUserAsync(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return ApiResponse<bool>.Failed(false, "User not found", StatusCodes.Status404NotFound, new List<string>());
        //    }

        //    var result = await _userManager.DeleteAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        var errors = result.Errors.Select(error => error.Description).ToList();
        //        return ApiResponse<bool>.Failed(false, "Failed to delete user", StatusCodes.Status400BadRequest, errors);
        //    }

        //    _userRepository.DeleteAsync(user);
        //    await _userRepository.SaveChangesAsync();

        //    return ApiResponse<bool>.Success(true, "User deleted successfully", StatusCodes.Status200OK);
        //}

        public async Task<ApiResponse<bool>> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            
                return ApiResponse<bool>.Failed(false, "User not found", StatusCodes.Status404NotFound, new List<string>());

                  _userRepository.DeleteAsync(user);
               await _userRepository.SaveChangesAsync();
            return ApiResponse<bool>.Success(true, "User deleted successfully", StatusCodes.Status200OK);            
        }
    }
}
