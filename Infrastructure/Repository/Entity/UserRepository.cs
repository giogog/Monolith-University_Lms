using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class UserRepository:IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> AddToRoles(User user, string[] roles)
    {
        return await _userManager.AddToRolesAsync(user, roles);
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password); 
    }

    public async Task<IdentityResult> CreateUser(User user,string passord)
    {
        return await _userManager.CreateAsync(user, passord);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<User> GetUser(Expression<Func<User, bool>> expression)
    {
        return await _userManager.Users.FirstOrDefaultAsync(expression);
    }
}
