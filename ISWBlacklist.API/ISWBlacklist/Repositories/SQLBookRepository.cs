using ISWBlacklist.Domain.Entities;
// using Microsoft.EntityFrameworkCore;

namespace ISWBlacklist.API.ISWBlacklist.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        public async Task<List<Book>> CreateBook(Book book)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book?>> DeleteBookById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book>> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book?>> GetBookById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book?>> UpdateBookById(Guid id, Book book)
        {
            throw new NotImplementedException();
        }
    }
}