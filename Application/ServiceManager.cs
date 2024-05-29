using AutoMapper;
using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthorizationService> _authorizationService;
    private readonly Lazy<IEmailService> _emailService;
    private readonly Lazy<IExamsService> _examsService;
    private readonly Lazy<IThirdpartyService> _thirdpartyService;
    private readonly Lazy<IUniversityService> _universityService;
    private readonly Lazy<IApplicantService> _applicantService;
    private readonly Lazy<ISubjectService> _subjectService;
    private readonly Lazy<ISeminarService> _seminarService;
    private readonly Lazy<ILectureService> _lectureService;
    private readonly Lazy<IGradeService> _gradeService;
    public ServiceManager(
        IRepositoryManager repositoryManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ITokenGenerator tokenGenerator,
        IEmailSender emailSender,
        HttpClient httpClient,
        IMapper mapper
        )
    {
        _authorizationService = new (() => new AuthorizationService(tokenGenerator, repositoryManager));
        _emailService = new (() => new EmailService(userManager, emailSender, repositoryManager, tokenGenerator));
        _examsService = new (() => new ExamService(httpClient));
        _thirdpartyService = new (() => new ThirdpartyService());
        _universityService = new (() => new UniversityService(repositoryManager));
        _applicantService = new(() => new ApplicantService());
        _subjectService = new(() => new SubjectService(repositoryManager, mapper));
        _seminarService = new Lazy<ISeminarService>(() => new SeminarService(repositoryManager, mapper));
        _lectureService = new Lazy<ILectureService>(() => new LectureService(repositoryManager, mapper));
        _gradeService = new(() => new GradeService());
    }
    public IAuthorizationService AuthorizationService => _authorizationService.Value;
    public IEmailService EmailService => _emailService.Value;
    public IExamsService ExamsService => _examsService.Value;
    public IThirdpartyService ThirdpartyService => _thirdpartyService.Value;
    public IUniversityService UniversityService => _universityService.Value;
    public ISubjectService SubjectService => _subjectService.Value;
    public IApplicantService ApplicantService => _applicantService.Value;
    public ILectureService LectureService => _lectureService.Value;
    public ISeminarService SeminarService => _seminarService.Value;
    public IGradeService GradeService => _gradeService.Value;
}
