using Microsoft.AspNetCore.Identity;

namespace Web.Data;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
    {
        var administratorRole = new IdentityRole("Administrator");

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var basicRole = new IdentityRole("Basic");

        if (roleManager.Roles.All(r => r.Name != basicRole.Name))
        {
            await roleManager.CreateAsync(basicRole);
        }

        var administrator = new ApplicationUser { UserName = config["DefaultUser:Username"], Email = config["DefaultUser:Email"], EmailConfirmed = true };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, config["DefaultUser:Password"]);
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }
    }
}
