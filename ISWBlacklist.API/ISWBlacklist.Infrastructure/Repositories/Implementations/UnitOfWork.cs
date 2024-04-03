using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;

namespace ISWBlacklist.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlackListDbContext _dbContext;

        public UnitOfWork(BlackListDbContext dbContext)
        {
            _dbContext = dbContext;
            BookRepository = new BookRepository(_dbContext);
            //BlackListRepository = new BlackListRepository(_dbContext);
        }

        public IBookRepository BookRepository { get; private set; }
        //public IBlackListRepository BlackListRepository { get; private set; }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public void Dispose() => _dbContext.Dispose();
    }
}
