using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ISWBlacklist.Infrastructure.Repositories.Implementations
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(BlackListDbContext dbContext) : base(dbContext) { }

        public async Task AddUserAsync(AppUser user) => await AddAsync(user);
        public void UpdateUser(AppUser user) => Update(user);

        public async Task<AppUser> GetUserByIdAsync(string id) => await GetByIdAsync(id);

        public async Task<List<AppUser>> GetAllUsersAsync() =>  await GetAllAsync();

        public async Task<List<AppUser>> FindUsersAsync(Expression<Func<AppUser, bool>> condition) => await FindByConditionAsync(condition);

        public async Task<bool> UserExistsAsync(Expression<Func<AppUser, bool>> condition) =>  await ExistsAsync(condition);

        public async Task DeleteUserAsync(AppUser user)
        {
            Delete(user);
            await SaveChangesAsync();
        }
    }
}
