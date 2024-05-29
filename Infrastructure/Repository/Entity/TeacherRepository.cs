using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class TeacherRepository(DomainDataContext context) : BaseRepository<Teacher>(context), ITeacherRepository
{
    public async Task AddTeacher(Teacher teacher) => Create(teacher);

    public async Task DeleteTeacher(Teacher teacher) => Delete(teacher);

    public async Task UpdateTeacher(Teacher teacher) => Update(teacher);

    public async Task<IEnumerable<Teacher>> GetAllTeachers() =>
        await FindAll().OfType<Teacher>().ToArrayAsync();

    public async Task<Teacher> GetTeacherById(int id) =>
        await FindByCondition(t => t.Id == id).OfType<Teacher>()
        .FirstOrDefaultAsync();

    public IQueryable<Teacher> GetByCondition(Expression<Func<Teacher, bool>> expression) => FindByCondition(expression);
}
