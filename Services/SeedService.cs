using Microsoft.AspNetCore.Identity;
using Healthy_Recipes.Data;
using Healthy_Recipes.Models;
using System.Threading.Tasks;

namespace Healthy_Recipes.Services
{
    public static class SeedService
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Cr�er les r�les
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Cr�er l'utilisateur admin
            var adminEmail = "admin@healthyrecipes.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new Users
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrateur"
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Cr�er un utilisateur normal
            var userEmail = "user@healthyrecipes.com";
            var normalUser = await userManager.FindByEmailAsync(userEmail);
            if (normalUser == null)
            {
                normalUser = new Users
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FullName = "Utilisateur"
                };
                var result = await userManager.CreateAsync(normalUser, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }

            // Ajouter des recettes de test
            if (!context.Recipes.Any())
            {
                context.Recipes.AddRange(
                    new Recipe
                    {
                        Title = "Salade de quinoa aux l�gumes",
                        Description = "Une salade riche en prot�ines et pleine de saveurs avec des l�gumes de saison.",
                        Category = "V�g�tarien",
                        PreparationTime = 20,
                        ImageUrl = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?ixlib=rb-4.0.3&auto=format&fit=crop&w=880&q=80",
                        Ingredients = "1 tasse de quinoa cuit\n1 concombre\n2 tomates\n1 avocat\n1/4 tasse d'olives\n2 c. � soupe d'huile d'olive\nJus de citron\nSel et poivre",
                        Instructions = "Rincer le quinoa � l'eau froide, puis le faire cuire selon les instructions sur l'emballage.\nPendant que le quinoa cuit, couper les l�gumes en d�s de taille similaire.\nDans un grand bol, m�langer le quinoa cuit et refroidi avec les l�gumes.\nArroser d'huile d'olive et de jus de citron, puis assaisonner avec du sel et du poivre.\nM�langer d�licatement et servir imm�diatement ou r�frig�rer jusqu'au moment de servir.",
                        Calories = 320,
                        Protein = 12,
                        Carbohydrates = 35,
                        Fat = 15
                    },
                    new Recipe
                    {
                        Title = "Bowl prot�in� au saumon",
                        Description = "Un bowl �quilibr� avec du saumon grill�, avocat, quinoa et l�gumes croquants.",
                        Category = "Poisson",
                        PreparationTime = 25,
                        ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?ixlib=rb-4.0.3&auto=format&fit=crop&w=880&q=80",
                        Ingredients = "150g de saumon frais\n1/2 tasse de quinoa cuit\n1/2 avocat\n1/2 concombre\n1 carotte\n1 c. � soupe de graines de s�same\n1 c. � soupe de sauce soja\n1 c. � soupe d'huile d'olive",
                        Instructions = "Faire cuire le quinoa selon les instructions.\nAssaisonner le saumon avec du sel et du poivre, puis le faire griller � la po�le avec un peu d'huile d'olive.\nCouper les l�gumes en fines lamelles ou en d�s.\nDans un bol, disposer le quinoa, les l�gumes et le saumon.\nSaupoudrer de graines de s�same et arroser de sauce soja.",
                        Calories = 450,
                        Protein = 30,
                        Carbohydrates = 40,
                        Fat = 20
                    },
                    new Recipe
                    {
                        Title = "Smoothie vert �nergisant",
                        Description = "Un smoothie revitalisant � base d'�pinards, banane, pomme et graines de chia.",
                        Category = "Vegan",
                        PreparationTime = 5,
                        ImageUrl = "https://images.unsplash.com/photo-1490645935967-10de6ba17061?ixlib=rb-4.0.3&auto=format&fit=crop&w=880&q=80",
                        Ingredients = "1 poign�e d'�pinards frais\n1 banane\n1/2 pomme\n1 c. � caf� de graines de chia\n200ml de lait d'amande\n1 c. � caf� de miel (optionnel)",
                        Instructions = "Laver les �pinards et les mettre dans le blender.\nAjouter la banane coup�e en morceaux et la pomme.\nVerser le lait d'amande et les graines de chia.\nMixer jusqu'� obtenir une texture lisse.\nGo�ter et ajouter du miel si besoin de plus de douceur.",
                        Calories = 200,
                        Protein = 5,
                        Carbohydrates = 30,
                        Fat = 8
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}