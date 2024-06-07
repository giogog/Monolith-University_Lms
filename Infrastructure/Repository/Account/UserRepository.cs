using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
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

    public async Task<IdentityResult> AddToRole(User user, string role)
    {
        
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IdentityResult> DeleteUser(User user)
    {

        return await _userManager.DeleteAsync(user);
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password); 
    }

    public async Task<IdentityResult> CreateUser(User user,string passord)
    {
        return await _userManager.CreateAsync(user, passord);
    }

    public async Task<User> GetApplicantWithExamResultsAsync(Expression<Func<User, bool>> expression) =>
            await _userManager.Users.Where(expression)
            .Include(r => r.ExamResults) // Include ExamResults here
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userManager.Users.Include(u=>u.ExamResults).ToListAsync();
    }


    public async Task<User> GetUser(Expression<Func<User, bool>> expression)
    {
        return await _userManager.Users.FirstOrDefaultAsync(expression);
    }
}
