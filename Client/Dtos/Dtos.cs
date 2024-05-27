namespace Client.Dtos;

public record RegisterDto
{
    public string PersonalID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Faculty { get; set; }
}

public record LoginResult
{
    public string Token { get; set; }
    public string Username { get; set; } // This property is not needed anymore
}

public class LoginDto
{
    public string PersonalId { get; set; }
    public string Password { get; set; }
}

//public record LoginDto
//{
//    public string PersonalID { get; set; }
//    public string Password { get; set; }
//}
public record LoginResponseDto(string PersonalId, string Token);