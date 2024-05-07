using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly DomainDataContext _context;
    private readonly Lazy<IUserRepository> _userRepository;
    public RepositoryManager(DomainDataContext context,UserManager<User> userManager)
    {
        _context = context;
        _userRepository = new(() => new UserRepository(userManager));
    }
    public IUserRepository UserRepository => _userRepository.Value;

    public Task SaveAsync() => _context.SaveChangesAsync();
}
