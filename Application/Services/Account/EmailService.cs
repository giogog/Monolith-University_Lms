using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services;
public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager _repoManager;
    private readonly ITokenGenerator _tokenGenerator;
    public EmailService(UserManager<User> userManager,
        IEmailSender emailSender,
        IRepositoryManager repoManager,
        ITokenGenerator tokenGenerator
        )
    {

        _userManager = userManager;
        _emailSender = emailSender;
        _repoManager = repoManager;
        _tokenGenerator = tokenGenerator;
    }
    public async Task<IdentityResult> SendConfirmationMail(IUrlHelper urlHelper, string username)
    { 
        // Retrieve the user from the repository
        var user = await _repoManager.UserRepository.GetUser(user => user.UserName == username);
        if (user == null)
        {
            // Handle the case where the user is not found
            return IdentityResult.Failed(new IdentityError { Code = "UserDoesNotExist", Description = "The user does not exists." });
        }

        // Generate the email confirmation token
        var token = await _tokenGenerator.GenerateMailTokenCode(user);
        if (string.IsNullOrEmpty(token))
        {
            // Handle the case where the token generation fails
            return IdentityResult.Failed(new IdentityError { Code = "TokenGeneration", Description = "Token generation failed." });
          
        }

        // Generate the callback URL


        var callbackUrl = urlHelper.Action(
            action: "ConfirmEmail",
            controller: "Account",
            values: new { userId = user.Id, token },
            protocol: "https");

        if (string.IsNullOrEmpty(callbackUrl))
        {
            // Handle the case where the URL generation fails
            return IdentityResult.Failed(new IdentityError { Code = "CallBackUrlError", Description = "Callback URL generation failed." });
            
        }

        // Send the confirmation email
        return await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

    }




    // Method to confirm the user's email
    public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found." });
        }
        return await _userManager.ConfirmEmailAsync(user, token);
    }
}
