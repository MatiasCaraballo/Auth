using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
[ApiController]
[Route("[controller]")]
public class LoginJWTController : ControllerBase
{
    private readonly ILoginJWTService _loginJWTService;
    private readonly ILogger<LoginJWTController> _logger;


    public LoginJWTController(ILoginJWTService loginJWTService, ILogger<LoginJWTController> logger)
    {
        _loginJWTService = loginJWTService;
        _logger = logger; 
    }

    [HttpPost()]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var loginResponse = await _loginJWTService.Login(loginDto);
            return Ok(new { message = "Login successful", token = loginResponse.Token});

        }
         catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error Login the user",loginDto.Username);
            return StatusCode(500, new { message = "Unexpected error" });
        }
    
    }
}