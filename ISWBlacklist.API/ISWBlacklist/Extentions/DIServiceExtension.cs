using ISWBlacklist.Application.Services;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Implementations;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISWBlacklist.Extentions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddDbContext<BlackListDbContext>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection")));
            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<BlackListDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
