namespace Contracts;

public interface IServiceManager
{
    IEmailService EmailService { get; }
    IAuthorizationService AuthorizationService { get; }

}
