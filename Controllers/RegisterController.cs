using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private readonly IRegisterService _registerService;

    public RegisterController(IRegisterService registerService)
    {
        _registerService = registerService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        var result = await _registerService.RegisterAsync(registerUserDTO);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User successfully created" });
    }
}
