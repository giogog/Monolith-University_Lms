using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Contracts;

public interface IEmailService
{
    
    Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
    Task<IdentityResult> SendConfirmationMail(IUrlHelper urlHelper, string username);
}
