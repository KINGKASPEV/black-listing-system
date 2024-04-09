using ISWBlacklist.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ISWBlacklist.Common.Utilities
{
    public class Seeder
    {
        public static async Task SeedRolesAndAdmins(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Seed roles
            await SeedRole(roleManager, "UserAdmin");
            await SeedRole(roleManager, "BlacklistAdmin");
            await SeedRole(roleManager, "User");

            // Seed admin user
            await SeedAdminUser(userManager, "UserAdmin", "useradmin@gmail.com", "Password@123", "UserAdmin");
        }

        private static async Task SeedRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task SeedAdminUser(UserManager<AppUser> userManager, string roleName, string email, string password, string firstName)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
