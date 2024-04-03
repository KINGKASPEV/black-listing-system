namespace ISWBlacklist.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository BookRepository { get; }
        //IBlackListRepository BlackListRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
