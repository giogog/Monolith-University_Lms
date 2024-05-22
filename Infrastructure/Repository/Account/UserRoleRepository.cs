using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserRoleRepository(DomainDataContext context) : BaseRepository<UserRole>(context), IUserRoleRepository
{
    public async Task DeleteRoleToUserAsync(int userId, int roleId) => 
        Delete(await FindAll().Where(ur => ur.RoleId == roleId && ur.UserId == userId).FirstOrDefaultAsync());

    public async Task<User> GetUserByRoleAsync(int roleId) => 
        await FindByCondition(r => r.RoleId == roleId)
        .Select(r => r.User).FirstOrDefaultAsync();

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId) =>
        await FindByCondition(r => r.RoleId == roleId)
        .Select(r => r.User).ToArrayAsync();
}
