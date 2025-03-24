using BudgetMan.Enums;
using BudgetMan.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BudgetMan.Data
{
    public static class DBInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, 
                                         UserManager<User> userManager, 
                                         RoleManager<IdentityRole> roleManager)
        {
            // 1. Vérification et création des rôles
            foreach (var role in Enum.GetNames(typeof(Role)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"Rôle créé : {role}");
                }
            }

            // 2. Création de l'admin avec gestion d'erreur
            var adminEmail = "budgetmanadmin@hotmail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,  // Important pour bypasser la vérification
                    FirstName = "Ad",
                    LastName = "Min",
                    Country = "Canada"
                };

                var createAdmin = await userManager.CreateAsync(admin, "Password123!");
                
                if (createAdmin.Succeeded)
                {
                    Console.WriteLine("Admin créé avec succès");
                    await userManager.AddToRoleAsync(admin, Role.Admin.ToString());
                }
                else
                {
                    Console.WriteLine($"Erreur création admin: {string.Join(", ", createAdmin.Errors)}");
                }
            }

            // 3. Création de l'utilisateur test
            var userEmail = "budgetmanusertest@hotmail.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new User
                {
                    UserName = "usertest",
                    Email = userEmail,
                    EmailConfirmed = true,
                    FirstName = "User",
                    LastName = "Test",
                    Country = "Canada"
                };

                var createUser = await userManager.CreateAsync(user, "Password123!");
                
                if (createUser.Succeeded)
                {
                    Console.WriteLine("Utilisateur test créé avec succès");
                    await userManager.AddToRoleAsync(user, Role.User.ToString());
                }
                else
                {
                    Console.WriteLine($"Erreur création utilisateur: {string.Join(", ", createUser.Errors)}");
                }
            }
        }
    }
}