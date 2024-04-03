using ISWBlacklist.Domain.Entities;
using System.Linq.Expressions;

namespace ISWBlacklist.Infrastructure.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(Book book);
        Task<List<Book>> FindBookAsync(Expression<Func<Book, bool>> condition);
        Task<Book> GetBookByIdAsync(string id);
        Task UpdateBookAsync(Book book);
    }
}
