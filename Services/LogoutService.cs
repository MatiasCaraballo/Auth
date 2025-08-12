using Microsoft.AspNetCore.Identity;

public class LogoutService : ILogoutService
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LogoutService(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<bool> Logout()
    {
        await _signInManager.SignOutAsync();
        return true;
    }
}