namespace ISWBlacklist.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository BookRepository { get; }
        IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
