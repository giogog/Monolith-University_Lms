
public interface ITokenService
{
    Task DeleteTokenAndUsernameAsync();
    Task<(string Token, string Username)> GetTokenAndUsernameAsync();
    Task SaveTokenAsync(string token, string username);
}