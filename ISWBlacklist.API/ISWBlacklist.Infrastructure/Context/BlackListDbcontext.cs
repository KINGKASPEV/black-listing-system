using ISWBlacklist.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ISWBlacklist.Infrastructure.Context
{
    public class BlackListDbContext : IdentityDbContext<AppUser>
    {
        public BlackListDbContext(DbContextOptions<BlackListDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships
            modelBuilder.Entity<Book>()
                .HasOne(b => b.User)
                .WithMany(u => u.Book)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);
        }
    }
}
