using ISWBlacklist.Application;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Implementations;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISWBlacklist.Extentions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddDbContext<BlackListDbContext>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection")));
        }
    }
}
