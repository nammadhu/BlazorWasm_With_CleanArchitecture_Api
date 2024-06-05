using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicCommon;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Seeds
    {
    public static class DefaultRoles
        {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
            {
            //Seed Roles
            await AddRole(roleManager, CONSTANTS.Auth.Role_Admin);
            await AddRole(roleManager, CONSTANTS.Auth.Role_InternalAdmin);
            await AddRole(roleManager, CONSTANTS.Auth.Role_InternalViewer);
            await AddRole(roleManager, CONSTANTS.Auth.Role_CardCreator);

            await AddRole(roleManager, CONSTANTS.Auth.Role_CardOwner);
            await AddRole(roleManager, CONSTANTS.Auth.Role_CardReviewer);
            await AddRole(roleManager, CONSTANTS.Auth.Role_Blocked);
            //Creator
            //Any AuthenticatedUser
            //Anonymous
            static async Task AddRole(RoleManager<ApplicationRole> roleManager, string roleName)
                {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }
        }
    }
