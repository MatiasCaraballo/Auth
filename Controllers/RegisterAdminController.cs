/*using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RegisterAdminController : ControllerBase
{
    private readonly IRegisterAdminService _registerAdmin;

    public RegisterAdminController(IRegisterAdminService registerAdmin)
    {
        _registerAdmin = registerAdmin;
    }

[HttpPost("register-admin")]
public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDTO dto)
{
    try
    {
        var result = await _registerAdmin.RegisterAdmin(dto);

        return Ok(new { Message = "Admin successful created" });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            Message = "Error creating the admin user",
            Details = ex.Message
        });
    }
}
}*/