using Domain.Models;
using System.Linq.Expressions;

namespace Contracts;

public interface ISubjectRepository
{
    Task AddSubject(Subject subject);
    Task DeleteSubject(Subject subject);
    Task UpdateSubject(Subject subject);
    Task<Subject> GetSubjectById(int id);
    Task<IEnumerable<Subject>> GetAllSubjects();
    IQueryable<Subject> GetByCondition(Expression<Func<Subject, bool>> expression);


}
