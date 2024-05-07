using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Contracts;

public interface IUserRepository
{
    Task<IdentityResult> CreateUser(User user, string passord);
    Task<User> GetUser(Expression<Func<User, bool>> expression);
    Task<IEnumerable<User>> GetAllUsers();
    Task<IdentityResult> AddToRoles(User user, string[] roles);
    Task<bool> CheckPassword(User user,string password);
}
