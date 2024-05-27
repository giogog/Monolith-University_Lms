using Domain.Models;

namespace Contracts;

public interface IRoleRepository
{
    Task<Role> GetRoleByIdAsync(int id);
    Task<Role> GetRoleByNameAsync(string name);
    Task<bool> RoleExists(string name);
}
