using Domain.Models;
using System.Linq.Expressions;

namespace Contracts;

public interface IEnrollmentRepository
{
    Task AddEnrollment(StudentEnrollment studentEnrollment);
    Task UpdateEnrollment(StudentEnrollment studentEnrollment);
    Task DeleteEnrollment(StudentEnrollment studentEnrollment);
    Task<StudentEnrollment> GetEnrollmentByIds(Expression<Func<StudentEnrollment, bool>> expression);
    Task<IEnumerable<StudentEnrollment>> GetAllEnrollments();
    Task<IEnumerable<StudentEnrollment>> GetAllEnrollmentByIds(Expression<Func<StudentEnrollment, bool>> expression);
    IQueryable<StudentEnrollment> GetByCondition(Expression<Func<StudentEnrollment, bool>> expression);
}
