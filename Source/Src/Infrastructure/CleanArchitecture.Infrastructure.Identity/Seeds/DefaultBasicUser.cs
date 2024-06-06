using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicCommon;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Seeds;

public static class DefaultBasicUser
    {
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
        //Seed Default User
        var defaultUser = new ApplicationUser
            {
            UserName = CONSTANTS.Auth.Role_Admin,
            Email = "Admin@Admin.com",
            Name = "Nammadhu",
            PhoneNumber = "9964548789",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
            };

        if (!await userManager.Users.AnyAsync())
            {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
                {
                await userManager.CreateAsync(defaultUser, "Sam@12345");
                await userManager.AddToRoleAsync(defaultUser, CONSTANTS.Auth.Role_Admin);
                }

            }
        }
    }

