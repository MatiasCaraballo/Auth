using Microsoft.AspNetCore.Identity;

public interface IRegisterService
{
    Task<IdentityResult> RegisterAsync(RegisterUserDTO registerUserDTO);
}
