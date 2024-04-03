using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ISWBlacklist.Infrastructure.Repositories.Implementations
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(BlackListDbContext dbContext) : base(dbContext) { }

        public async Task AddBookAsync(Book book) => await AddAsync(book);

        public async Task<List<Book>> FindBookAsync(Expression<Func<Book, bool>> condition) => await FindByConditionAsync(condition);

        public async Task<Book> GetBookByIdAsync(string id) => await GetByIdAsync(id);

        public async Task<List<Book>> GetAllBooksAsync() => await GetAllAsync();

        public async Task UpdateBookAsync(Book book)
        {
            Update(book);
            await SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Book book)
        {
            Delete(book);
            await SaveChangesAsync();
        }
    }
}
