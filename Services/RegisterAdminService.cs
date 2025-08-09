using Microsoft.AspNetCore.Identity;
public class RegisterAdminService : IRegisterAdminService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public RegisterAdminService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<IResult> RegisterAdmin(RegisterAdminDTO dto)
    {
        
        var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
        var createUserResult = await _userManager.CreateAsync(user, dto.Password);
         if (!createUserResult.Succeeded){ return TypedResults.InternalServerError(); }

        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new IdentityRole("Admin"));

        var addRoleResult = await _userManager.AddToRoleAsync(user, "Admin");
        if(!addRoleResult.Succeeded){ return TypedResults.InternalServerError(); }

        return TypedResults.Created($"/auth");
    }
}