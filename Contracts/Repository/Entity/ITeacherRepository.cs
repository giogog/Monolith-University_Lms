using Domain.Models;
using System.Linq.Expressions;

namespace Contracts;

public interface ITeacherRepository
{
    Task AddTeacher(Teacher teacher);
    Task DeleteTeacher(Teacher teacher);
    Task UpdateTeacher(Teacher teacher);
    Task<Teacher> GetTeacherById(int id);
    Task<IEnumerable<Teacher>> GetAllTeachers();
    IQueryable<Teacher> GetByCondition(Expression<Func<Teacher, bool>> expression);

}
