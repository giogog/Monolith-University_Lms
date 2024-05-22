using Application.Services;
using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthorizationService> _authorizationService;
    private readonly Lazy<IEmailService> _emailService;
    private readonly Lazy<IExamsService> _examsService;
    private readonly Lazy<IThirdpartyService> _thirdpartyService;
    private readonly Lazy<IUniversityService> _universityService;
    public ServiceManager(
        IRepositoryManager repositoryManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ITokenGenerator tokenGenerator,
        IEmailSender emailSender,
        HttpClient httpClient
        )
    {
        _authorizationService = new (() => new AuthorizationService(roleManager, tokenGenerator, repositoryManager));
        _emailService = new (() => new EmailService(userManager, emailSender, repositoryManager, tokenGenerator));
        _examsService = new (() => new ExamService(httpClient));
        _thirdpartyService = new (() => new ThirdpartyService());
        _universityService = new (() => new UniversityService(repositoryManager));
    }
    public IAuthorizationService AuthorizationService => _authorizationService.Value;
    public IEmailService EmailService => _emailService.Value;
    public IExamsService ExamsService => _examsService.Value;
    public IThirdpartyService ThirdpartyService => _thirdpartyService.Value;
    public IUniversityService UniversityService => _universityService.Value;
}
