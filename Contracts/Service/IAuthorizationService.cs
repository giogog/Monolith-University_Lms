using Domain.Models;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace Contracts;

public interface IAuthorizationService
{

    Task<IdentityResult> Login(LoginDto loginDto);
    Task<LoginResponseDto> Authenticate(Expression<Func<User, bool>> expression);
    Task<IdentityResult> Register(RegisterDto registerDto);
    Task<IdentityResult> AssignRole(User? user, string[] roles, string? username);
    Task<EmailResult> CheckMailConfirmation(string Username);



}
