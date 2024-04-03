using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;

namespace ISWBlacklist.Infrastructure.Repositories.Implementations
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(BlackListDbContext dbContext) : base(dbContext) { }
    }
}
