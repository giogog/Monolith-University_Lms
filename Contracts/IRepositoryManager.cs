namespace Contracts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }
    IUniversityRepository UniversityRepository { get; }
    IFacultyRepository FacultyRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    Task SaveAsync();
}
