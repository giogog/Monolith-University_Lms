namespace Contracts;

public interface IServiceManager
{
    IEmailService EmailService { get; }
    IAuthorizationService AuthorizationService { get; }
    IExamsService ExamsService { get; }
    IThirdpartyService ThirdpartyService { get; }
    IUniversityService UniversityService { get; }
    IApplicantService ApplicantService { get; }

}
