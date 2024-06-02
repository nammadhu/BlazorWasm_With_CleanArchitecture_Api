using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicCommon;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Seeds
    {
    public static class DefaultBasicUser
        {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
            {
            //Seed Default User
            var defaultUser = new ApplicationUser
                {
                UserName = CONSTANTS.Auth.Role_Admin,
                Email = "Madhusudhan.veerabhadrappa@gmail.com",
                Name = "Nammadhu",
                PhoneNumber = "9964548789",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
                };

            //if (!await userManager.Users.AnyAsync())
              //  {
              //always checks on every run due to above commented line
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                    {
                    await userManager.CreateAsync(defaultUser);
                    await userManager.AddToRoleAsync(defaultUser, CONSTANTS.Auth.Role_Admin);
                    }

                //}
            }
        }
    }
