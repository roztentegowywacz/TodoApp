using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Data;

namespace TodoApp
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();
            
            await EnsureRoleAsync(roleManager);

            var userManager = services
                .GetRequiredService<UserManager<ApplicationUser>>();

            await EnsureTestAdminAsync(userManager);   
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager
                .RoleExistsAsync(Constants.AdministratorRole);

                if (alreadyExists)
                {
                    return;
                }

                await roleManager.CreateAsync(
                    new IdentityRole(Constants.AdministratorRole)
                );
        }
        
        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync();

            if (testAdmin != null)
            {
                return;
            }

            testAdmin = new ApplicationUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };

            await userManager.CreateAsync(testAdmin, "Pass123!");

            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
        }
    }

}