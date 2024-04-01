using ISWBlacklist.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ISWBlacklist.Extentions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<BlackListDbContext>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection")));
        }
    }
}
