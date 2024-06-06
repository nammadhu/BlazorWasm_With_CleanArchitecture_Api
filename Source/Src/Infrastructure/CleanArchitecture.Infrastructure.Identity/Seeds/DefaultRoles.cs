using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicCommon;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Seeds;

public static class DefaultRoles
    {
    public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
        //Seed Roles
        await AddRole(roleManager, CONSTANTS.Auth.Role_Admin);
        await AddRole(roleManager, CONSTANTS.Auth.Role_InternalAdmin);
        await AddRole(roleManager, CONSTANTS.Auth.Role_InternalViewer);

        static async Task AddRole(RoleManager<ApplicationRole> roleManager, string roleName)
            {
            if (!await roleManager.Roles.AnyAsync() && !await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }
    }

