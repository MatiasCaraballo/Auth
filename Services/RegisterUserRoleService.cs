
using Microsoft.AspNetCore.Identity;

public class RegisterUserRoleService : IRegisterUserRoleService
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly IConfiguration _configuration;

    public RegisterUserRoleService(UserManager<IdentityUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUserRole(RegisterUserRoleDTO registerUserRoleDTO)
    {
        var user = new IdentityUser
        {
            UserName = registerUserRoleDTO.Username,
            Email = registerUserRoleDTO.Email
        };

        var result = await _userManager.CreateAsync(user, registerUserRoleDTO.Password);

        if (!result.Succeeded){ return result;}

        string role = registerUserRoleDTO.SecretKey == _configuration["SecretKey"] ? "Admin" : "Client";

        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded){ return roleResult;}

        return result;

    }
}
