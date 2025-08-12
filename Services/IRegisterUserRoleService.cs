using Microsoft.AspNetCore.Identity;

public interface IRegisterUserRoleService
{
    Task<IdentityResult> RegisterUserRole(RegisterUserRoleDTO registerUserRoleDTO);
}
