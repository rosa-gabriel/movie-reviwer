using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public class SeedUser
    {
        public static async Task SeedDataBase(WebApplication app)
        {
            var scope = app.Services.CreateScope();

            var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
            var configuration = scope.ServiceProvider.GetService<IConfiguration>();
            var context = scope.ServiceProvider.GetService<DataContext>();

            await SeedUser.SeedAdminUser(userManager, context, configuration);
        }

        public static async Task SeedAdminUser(UserManager<User> userManager, DataContext context, IConfiguration config)
        {
            if (!await userManager.Users.AnyAsync(u => u.UserName == "Admin"))
            {
                User admin = new User
                {
                    UserName = "Admin",
                    Bio = "The app administrator.",
                    Email = config.GetSection("EmailAddress").Value,
                    ProfileImageUrl = "https://i.imgur.com/mggfJH8.png",
                    CreationDate = DateTime.Now,
                    IsAdmin = true,
                };
                await userManager.CreateAsync(admin, "4dm1nP4$$L0rd");

                string token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                await userManager.ConfirmEmailAsync(admin, token);
            }
        }
    }
}