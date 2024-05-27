using Blazored.SessionStorage;
using System.Threading.Tasks;

public class TokenService : ITokenService
{
    private readonly ISessionStorageService _sessionStorage;

    public TokenService(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async Task SaveTokenAsync(string token, string username)
    {
        await _sessionStorage.SetItemAsync("authToken", token);
        await _sessionStorage.SetItemAsync("username", username);
    }

    public async Task<(string Token, string Username)> GetTokenAndUsernameAsync()
    {
        var token = await _sessionStorage.GetItemAsync<string>("authToken");
        var username = await _sessionStorage.GetItemAsync<string>("username");
        return (token, username);
    }

    public async Task DeleteTokenAndUsernameAsync()
    {
        await _sessionStorage.RemoveItemAsync("authToken");
        await _sessionStorage.RemoveItemAsync("username");
    }
}
