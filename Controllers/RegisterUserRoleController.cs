using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class RegisterUserRoleController : ControllerBase
{
    private readonly IRegisterUserRoleService _registerUserRoleService;
    private readonly ILogger<RegisterUserRoleController> _logger;

    public RegisterUserRoleController(IRegisterUserRoleService registerService, ILogger<RegisterUserRoleController> logger)
    {
        _registerUserRoleService = registerService;
        _logger = logger;
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Register([FromBody] RegisterUserRoleDTO registerUserRoleDTO)
    {
        try
        {
            var result = await _registerUserRoleService.RegisterUserRole(registerUserRoleDTO);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "User successfully created" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error registering {Email}", registerUserRoleDTO.Email);
            return StatusCode(500, new { message = "Unexpected error" });

        }
    }
}
