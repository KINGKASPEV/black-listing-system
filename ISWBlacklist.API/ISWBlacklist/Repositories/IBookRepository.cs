using ISWBlacklist.Domain.Entities;

namespace ISWBlacklist.API.ISWBlacklist.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooks();
        Task<List<Book>> CreateBook(Book book);
        Task<List<Book?>> GetBookById(Guid id);
        Task<List<Book?>> UpdateBookById(Guid id,Book book);
        Task<List<Book?>> DeleteBookById(Guid id);
    }
}