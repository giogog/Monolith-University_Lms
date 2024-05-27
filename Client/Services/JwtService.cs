using System.IdentityModel.Tokens.Jwt;
using System.Linq;

public class JwtService : IJwtService
{
    public string GetRoleFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        return roleClaim;
    }

    public string GetUsernameFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
        return usernameClaim;
    }
}
