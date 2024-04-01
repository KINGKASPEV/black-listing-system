using ISWBlacklist.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISWBlacklist.Infrastructure.Extentions
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
