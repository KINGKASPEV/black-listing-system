using ISWBlacklist.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ISWBlacklist.Infrastructure.Context
{
    public class BlackListDbContext : IdentityDbContext<AppUser>
    {
        public BlackListDbContext(DbContextOptions<BlackListDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .Property(item => item.Price)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
