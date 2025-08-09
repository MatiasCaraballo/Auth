public interface IRegisterAdminService
{
    Task<IResult> RegisterAdmin(RegisterAdminDTO dto);
}