using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;

public class RegisterService : IRegisterService
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly IConfiguration _configuration;

    public RegisterService(UserManager<IdentityUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterUserDTO registerUserDTO)
    {
        var user = new IdentityUser
        {
            UserName = registerUserDTO.Username,
            Email = registerUserDTO.Email
        };

        var result = await _userManager.CreateAsync(user, registerUserDTO.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        string role = registerUserDTO.SecretKey == _configuration["SecretKey"] ? "Admin" : "Client";

        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            return roleResult; 
        }

        return result;

    }
}
