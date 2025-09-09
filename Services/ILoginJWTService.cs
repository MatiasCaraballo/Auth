public interface ILoginJWTService
{
    Task<(string Token, DateTime Expiration)> Login(LoginDto dto);
}