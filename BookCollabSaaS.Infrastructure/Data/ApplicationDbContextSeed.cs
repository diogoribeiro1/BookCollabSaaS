using System;
using System.Threading.Tasks;
using BookCollabSaaS.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace BookCollabSaaS.Infrastructure.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                // Create roles
                var adminRole = new RoleEntity("Admin");
                var userRole = new RoleEntity("User");

                await context.Roles.AddRangeAsync(adminRole, userRole);
                await context.SaveChangesAsync();

                // Create users
                var adminUser = new UserEntity(
                    name: "Admin User",
                    email: "admin@site.com",
                    password: "SuperSecurePassword123"
                );
                adminUser.AddRole(adminRole);

                var regularUser = new UserEntity(
                    name: "Regular User",
                    email: "user@site.com",
                    password: "UserPassword456"
                );
                regularUser.AddRole(userRole);

                await context.Users.AddRangeAsync(adminUser, regularUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
