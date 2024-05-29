using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class EnrollmentRepository(DomainDataContext context) : BaseRepository<StudentEnrollment>(context), IEnrollmentRepository
{

    public async Task AddEnrollment(StudentEnrollment studentEnrollment) => Create(studentEnrollment);

    public async Task DeleteEnrollment(StudentEnrollment studentEnrollment) => Delete(studentEnrollment);

    public async Task UpdateEnrollment(StudentEnrollment studentEnrollment) => Update(studentEnrollment);

    public async Task<IEnumerable<StudentEnrollment>> GetAllEnrollmentByIds(Expression<Func<StudentEnrollment, bool>> expression) => 
        await FindByCondition(expression)
        .ToArrayAsync();
    public async Task<IEnumerable<StudentEnrollment>> GetAllEnrollments() => 
        await FindAll()
        .ToArrayAsync();

    public async Task<StudentEnrollment> GetEnrollmentByIds(Expression<Func<StudentEnrollment, bool>> expression) => 
        await FindByCondition(expression)
        .FirstOrDefaultAsync();

    public IQueryable<StudentEnrollment> GetByCondition(Expression<Func<StudentEnrollment, bool>> expression) => FindByCondition(expression);
}
