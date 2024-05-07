namespace Domain.Dtos;

public record LoginDto(string Username,string Password);
public record LoginResponseDto(string Username, string Token);

//public record RegisterDto(string Name, string Surname, string Username, string Email, string Password);

public record RegisterDto(string Username, string Email, string Password);
public record AssignRoleDto(string Username, string[] Roles);
