using ISWBlacklist.Domain.Entities;

namespace ISWBlacklist.Infrastructure.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<List<Book>> GetBooksByAuthorAsync(string author);
        Task<List<Book>> SearchBooksAsync(string searchTerm);
    }
}
