using ISWBlacklist.Domain.Entities;
// using Microsoft.EntityFrameworkCore;

namespace ISWBlacklist.API.ISWBlacklist.Repositories
{
    public class SQLUserRepository : IUserRepository
    {
       
        public Task<List<AppUser>> CreateUser(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppUser?>> DeleteUserById(Guid id)
        {
            throw new NotImplementedException();
        }


        public Task<List<AppUser>> GetAllUsers()
        {
            throw new NotImplementedException();
        }


        public Task<List<AppUser?>> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppUser?>> UpdateUserById(Guid id, AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}