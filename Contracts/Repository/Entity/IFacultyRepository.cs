using Domain.Models;
using System.Linq.Expressions;

namespace Contracts;

public interface IFacultyRepository
{
    Task AddFacultyAsync(Faculty faculty);
    Task UpdateFacultyAsync(Faculty faculty);
    Task DeleteFacultyAsync(Faculty faculty);
    IQueryable<Faculty> GetByCondition(Expression<Func<Faculty, bool>> expression);
    Task<Faculty> GetFacultyByNameAsync(string name);
    Task<IEnumerable<Faculty>> GetAllFacultiesAsync();
}
