using BudgetMan.Enums;
using BudgetMan.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BudgetMan.Data
{
    public static class DBInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Role.User.ToString()));
            }
            if (!await userManager.Users.AnyAsync())
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "budgetmanadmin@hotmail.com",
                    FirstName = "Ad",
                    LastName = "Min",
                    Country = "Canada"
                };
                await userManager.CreateAsync(admin, "Password123");
                await userManager.AddToRoleAsync(admin, Role.Admin.ToString());

                var user = new User
                {
                    UserName = "usertest",
                    Email = "budgetmanusertest@hotmail.com",
                    FirstName = "User",
                    LastName = "Test",
                    Country = "Canada"
                };
                await userManager.CreateAsync(user, "Password123");
                await userManager.AddToRoleAsync(user, Role.User.ToString());
            }
        }
    }
}
