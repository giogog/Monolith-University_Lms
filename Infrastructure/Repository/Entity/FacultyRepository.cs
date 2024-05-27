using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class FacultyRepository(DomainDataContext context) : BaseRepository<Faculty>(context), IFacultyRepository
{
    public async Task AddFacultyAsync(Faculty faculty) => Create(faculty);

    public async Task DeleteFacultyAsync(Faculty faculty) => Delete(faculty);

    public async Task UpdateFacultyAsync(Faculty faculty) => Update(faculty);

    public async Task<IEnumerable<Faculty>> GetAllFacultiesAsync() => await FindAll().ToArrayAsync();

    public IQueryable<Faculty> GetByCondition(Expression<Func<Faculty, bool>> expression) => FindByCondition(expression);

    public async Task<Faculty> GetFacultyByNameAsync(string name) => 
        await FindByCondition(f => f.Name.ToLower() == name.ToLower())
        .FirstOrDefaultAsync();
    
}
