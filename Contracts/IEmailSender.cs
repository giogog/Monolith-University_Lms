using Domain.Models;
using Microsoft.AspNetCore.Identity;
namespace Contracts;

public interface IEmailSender
{
    Task<IdentityResult> SendEmailAsync(string email, string subject, string htmlMessage);
}
