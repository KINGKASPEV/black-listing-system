using ISWBlacklist.Domain.Entities;
using System.Linq.Expressions;

namespace ISWBlacklist.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(AppUser user);
        void UpdateUser(AppUser user);
        Task<AppUser> GetUserByIdAsync(string id);
        Task<List<AppUser>> GetAllUsersAsync();
        Task<List<AppUser>> FindUsersAsync(Expression<Func<AppUser, bool>> condition);
        Task<bool> UserExistsAsync(Expression<Func<AppUser, bool>> condition);
        Task DeleteUserAsync(AppUser user);
    }
}
