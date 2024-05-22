using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public interface IUserRoleRepository
{
    Task<User> GetUserByRoleAsync(int roleId);
    Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
    Task DeleteRoleToUserAsync(int userId, int roleId);
}
