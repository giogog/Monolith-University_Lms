using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class StudentRepository(DomainDataContext context) : BaseRepository<Student>(context), IStudentRepository
{
    public async Task AddStudentAsync(Student student) =>
        Create(student);

    public async Task DeleteStudentAsync(Student student) =>
        Delete(student);

    public async Task UpdateStudentAsync(Student student) =>
        Update(student);
    public async Task<IEnumerable<Student>> GetAllStudentsAsync() =>
        await FindAll().OfType<Student>().ToArrayAsync();

    public async Task<Student> GetStudentByIdAsync(int id) =>
        await FindByCondition(p => p.Id == id).OfType<Student>()
        .FirstOrDefaultAsync();



    public IQueryable<Student> GetByConditionAsync(Expression<Func<Student, bool>> expression) => FindByCondition(expression);
}
