using ISWBlacklist.Domain.Entities;
using System.Collections.Generic;

namespace ISWBlacklist.Infrastructure.Context
{
    public class BlackListDbcontext : IdentityDbContext<AppUser>
    {
        public BlackListDbcontext(DbContextOptions<BlackListDbcontext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
    }
}
