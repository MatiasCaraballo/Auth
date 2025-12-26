using Microsoft.AspNetCore.Identity;

public static class RoleInitializer
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = {"Client","Admin" };
        //try
        //{
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        //}
        //catch (Exception)
        //{
          //  return StatusCode(500, "Error creating the roles");
        //}
    }
}