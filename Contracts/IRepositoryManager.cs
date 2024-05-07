namespace Contracts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }
    Task SaveAsync();
}
