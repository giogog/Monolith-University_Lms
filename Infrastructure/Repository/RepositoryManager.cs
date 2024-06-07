using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly DomainDataContext _context;
    private IDbContextTransaction _transaction;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IUniversityRepository> _universityRepository;
    private readonly Lazy<IFacultyRepository> _facultyRepository;
    private readonly Lazy<IUserRoleRepository> _userroleRepository;
    private readonly Lazy<IRoleRepository> _roleRepository;
    private readonly Lazy<IStudentRepository> _studentRepository;
    private readonly Lazy<ITeacherRepository> _teacherRepository;
    private readonly Lazy<ISubjectRepository> _subjectRepository;
    private readonly Lazy<ILectureRepository> _lectureRepository;
    private readonly Lazy<ISeminarRepository> _seminarRepository;
    private readonly Lazy<IEnrollmentRepository> _enrollmentRepository;

    public RepositoryManager(DomainDataContext context,UserManager<User> userManager,RoleManager<Role> roleManager)
    {
        _context = context;
        _userRepository = new(() => new UserRepository(userManager));
        _universityRepository = new(() => new UniversityRepository(context));
        _facultyRepository = new(() => new FacultyRepository(context));
        _userroleRepository = new(() => new UserRoleRepository(context));
        _roleRepository = new(() => new RoleRepository(roleManager));
        _studentRepository = new(() => new StudentRepository(context));
        _teacherRepository = new(() => new TeacherRepository(context));
        _subjectRepository = new(() => new SubjectRepository(context));
        _lectureRepository = new(() => new LectureRepository(context));
        _seminarRepository = new(() => new SeminarRepository(context));
        _enrollmentRepository = new(() => new EnrollmentRepository(context));
    }
    public IUserRepository UserRepository => _userRepository.Value;
    public IUniversityRepository UniversityRepository => _universityRepository.Value;
    public IFacultyRepository FacultyRepository => _facultyRepository.Value;
    public IUserRoleRepository UserRoleRepository => _userroleRepository.Value;
    public IStudentRepository StudentRepository => _studentRepository.Value;
    public ITeacherRepository TeacherRepository => _teacherRepository.Value;
    public ISeminarRepository SeminarRepository => _seminarRepository.Value;
    public ILectureRepository LectureRepository => _lectureRepository.Value;
    public IEnrollmentRepository EnrollmentRepository => _enrollmentRepository.Value;
    public ISubjectRepository SubjectRepository => _subjectRepository.Value;
    public IRoleRepository RoleRepository => _roleRepository.Value;
    public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();
    public async Task CommitTransactionAsync() => await _transaction.CommitAsync();
    public async Task RollbackTransactionAsync() => await _transaction.RollbackAsync();
    public void Dispose() => _transaction?.Dispose();
    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
}
