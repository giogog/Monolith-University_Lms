using Domain.Models;
using System.Linq.Expressions;

namespace Contracts;

public interface IStudentRepository
{
    Task AddStudentAsync(Student student);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(Student student);
    Task<Student> GetStudentByIdAsync(int id);
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    IQueryable<Student> GetByConditionAsync(Expression<Func<Student, bool>> expression);


}
