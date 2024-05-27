public interface IJwtService
{
    string GetRoleFromToken(string token);
    string GetUsernameFromToken(string token);
}