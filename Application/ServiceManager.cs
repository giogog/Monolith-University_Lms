using Application.Services;
using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthorizationService> _authorizationService;
    private readonly Lazy<IEmailService> _emailService;

    public ServiceManager(
        IRepositoryManager repositoryManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ITokenGenerator tokenGenerator,
        IEmailSender emailSender
        )
    {
        _authorizationService = new Lazy<IAuthorizationService>(() => new AuthorizationService(roleManager, tokenGenerator, repositoryManager));
        _emailService = new Lazy<IEmailService>(() => new EmailService(userManager, emailSender, repositoryManager, tokenGenerator));

    }
    public IAuthorizationService AuthorizationService => _authorizationService.Value;
    public IEmailService EmailService => _emailService.Value;
}
