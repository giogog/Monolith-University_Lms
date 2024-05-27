using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly DomainDataContext _context;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IUniversityRepository> _universityRepository;
    private readonly Lazy<IFacultyRepository> _facultyRepository;
    private readonly Lazy<IUserRoleRepository> _userroleRepository;
    private readonly Lazy<IRoleRepository> _roleRepository;
    private readonly Lazy<IStudentRepository> _studentRepository;
    public RepositoryManager(DomainDataContext context,UserManager<User> userManager,RoleManager<Role> roleManager)
    {
        _context = context;
        _userRepository = new(() => new UserRepository(userManager));
        _universityRepository = new(() => new UniversityRepository(context));
        _facultyRepository = new(() => new FacultyRepository(context));
        _userroleRepository = new(() => new UserRoleRepository(context));
        _roleRepository = new(() => new RoleRepository(roleManager));
        _studentRepository = new(() => new StudentRepository(context));
    }
    public IUserRepository UserRepository => _userRepository.Value;
    public IUniversityRepository UniversityRepository => _universityRepository.Value;
    public IFacultyRepository FacultyRepository => _facultyRepository.Value;
    public IUserRoleRepository UserRoleRepository => _userroleRepository.Value;
    public IStudentRepository StudentRepository => _studentRepository.Value;
    public IRoleRepository RoleRepository => _roleRepository.Value;

    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
}
