using ISWBlacklist.Domain.Entities;

namespace ISWBlacklist.API.ISWBlacklist.Repositories
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllUsers();
        Task<List<AppUser>> CreateUser(AppUser user);
        Task<List<AppUser?>> GetUserById(Guid id);
        Task<List<AppUser?>> UpdateUserById(Guid id,AppUser user);
        Task<List<AppUser?>> DeleteUserById(Guid id);
    }
}