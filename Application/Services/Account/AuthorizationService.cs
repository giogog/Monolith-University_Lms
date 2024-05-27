using Contracts;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Application.Services;

public class AuthorizationService : IAuthorizationService
{

    //private readonly RoleManager<Role> _roleManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IRepositoryManager _repositoryManager;



    public AuthorizationService(
        ITokenGenerator tokenGenerator,
        IRepositoryManager repositoryManager)
    {

        _tokenGenerator = tokenGenerator;
        _repositoryManager = repositoryManager;
    }

    public async Task<LoginResponseDto> Authenticate(Expression<Func<User, bool>> expression)
    {
        var user = await _repositoryManager.UserRepository.GetUser(expression);

        var token = await _tokenGenerator.GenerateToken(user);

        return new LoginResponseDto(user.UserName, token);
    }

    public async Task<IdentityResult> Register(RegisterDto registerDto)
    {
        var isDigit = registerDto.PersonalID.All(char.IsDigit);
        if (registerDto.PersonalID.Length != 11 && !isDigit)
            return IdentityResult.Failed(new IdentityError { Code = "WrongPersonalId", Description = "There is mistake in Personal Id" });

        var user = new User
        {
            UserName = registerDto.PersonalID,
            Email = registerDto.Email,
            RegistrationTime = DateTime.UtcNow,
            Surname = registerDto.Surname,
            Name = registerDto.Name,
            Faculty = registerDto.Faculty,
            ExamResults = registerDto.ExamResults
        };

        var result = await _repositoryManager.UserRepository.CreateUser(user, registerDto.Password);

        if (!result.Succeeded)
            return result;
        
        return await _repositoryManager.UserRepository.AddToRole(user, registerDto.Role.ToString());

    }

    public async Task<IdentityResult> Login(LoginDto loginDto)
    {

        var user = await _repositoryManager.UserRepository.GetUser(user => user.UserName == loginDto.PersonalId);

        if (user == null)
            return IdentityResult.Failed(new IdentityError { Code = "UserDoesNotExist", Description = "The user does not exists." });

        var PasswordCheck = await _repositoryManager.UserRepository.CheckPassword(user, loginDto.Password);
        if (!PasswordCheck)
            return IdentityResult.Failed(new IdentityError { Code = "IncorrectPassword", Description = "Incorrect password." });
        if (!user.EmailConfirmed)
            return IdentityResult.Failed(new IdentityError { Code = "MailisNotConfirmed", Description = "Please Confirm Mail." });

        return IdentityResult.Success;

    }


    public async Task<EmailResult> CheckMailConfirmation(string Username)
    {
        var user = await _repositoryManager.UserRepository.GetUser(u => u.UserName == Username);

        if (!user.EmailConfirmed)
            return new EmailResult { IsSuccess = false };

        return new EmailResult { IsSuccess = true, ErrorMessage = "Mail is Confirmed" };


    }
}