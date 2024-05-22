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
    public RepositoryManager(DomainDataContext context,UserManager<User> userManager)
    {
        _context = context;
        _userRepository = new(() => new UserRepository(userManager));
        _universityRepository = new(() => new UniversityRepository(context));
        _facultyRepository = new(() => new FacultyRepository(context));
        _userroleRepository = new(() => new UserRoleRepository(context));
    }
    public IUserRepository UserRepository => _userRepository.Value;
    public IUniversityRepository UniversityRepository => _universityRepository.Value;
    public IFacultyRepository FacultyRepository => _facultyRepository.Value;
    public IUserRoleRepository UserRoleRepository => _userroleRepository.Value;
    public Task SaveAsync() => _context.SaveChangesAsync();
}
