using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ISWBlacklist.Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BlackListDbContext _dbContext;

        public GenericRepository(BlackListDbContext dbContext) => _dbContext = dbContext;
        public async Task AddAsync(T entity) => await _dbContext.Set<T>().AddAsync(entity);
        public void DeleteAllAsync(List<T> entities) => _dbContext.Set<T>().RemoveRange(entities);
        public void DeleteAsync(T entity) => _dbContext.Set<T>().Remove(entity);
        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression) => await _dbContext.Set<T>().Where(expression).ToListAsync();
        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> expression) => await _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        public async Task<List<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();
        public async Task<T> GetByIdAsync(string id) => await _dbContext.Set<T>().FindAsync(id);
        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
        public void Update(T entity) => _dbContext.Set<T>().Update(entity);
    }
}