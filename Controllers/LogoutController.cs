using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class LogoutController : ControllerBase
{
    private readonly ILogoutService _logoutService;

    public LogoutController(ILogoutService logoutService)
    {
        _logoutService = logoutService;
    }

    [HttpPost()]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] object empty)
    {
        if (empty != null)
        {
            await _logoutService.Logout();
            return Ok(new { message = "Logout successful" });
        }
        return Unauthorized(new { message = "Invalid request" });
    }
}