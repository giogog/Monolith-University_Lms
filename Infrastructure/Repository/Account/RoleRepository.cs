using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repository;

public class RoleRepository:IRoleRepository
{
    private readonly RoleManager<Role> _roleManager;

    public RoleRepository(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Role> GetRoleByIdAsync(int id) => await _roleManager.FindByIdAsync(id.ToString());

    public async Task<Role> GetRoleByNameAsync(string name) => await _roleManager.FindByNameAsync(name);


    public async Task<bool> RoleExists(string name) => await _roleManager.RoleExistsAsync(name);
}
