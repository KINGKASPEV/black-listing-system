using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISWBlacklist.Infrastructure.Repositories.Implementations
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(BlackListDbContext dbContext) : base(dbContext) { }

        public async Task<List<Book>> GetBooksByAuthorAsync(string author) => await FindAsync(b => b.Author == author);
        public async Task<List<Book>> SearchBooksAsync(string searchTerm) => await FindAsync(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));
    }
}
