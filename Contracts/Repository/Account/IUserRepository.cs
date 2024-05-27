using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Contracts;

public interface IUserRepository
{
    Task<IdentityResult> CreateUser(User user, string passord);
    Task<IdentityResult> DeleteUser(User user);
    Task<User> GetUser(Expression<Func<User, bool>> expression);
    Task<IEnumerable<User>> GetAllUsers();
    Task<IdentityResult> AddToRole(User user, string roles);
    Task<bool> CheckPassword(User user,string password);
    Task<User> GetApplicantWithExamResultsAsync(Expression<Func<User, bool>> expression);
}
