using ISWBlacklist.Application.Services.Implementations;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Implementations;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ISWBlacklist.Configurations
{
    public static class AuthenticationServiceExtension
    {
        public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtTokenGeneratorService, JwtTokenGeneratorService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
