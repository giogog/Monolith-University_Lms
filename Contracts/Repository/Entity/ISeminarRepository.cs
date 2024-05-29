using Domain.Models;
using System.Linq.Expressions;
namespace Contracts;

public interface ISeminarRepository
{
    Task AddSeminar(Seminar seminar);
    Task UpdateSeminar(Seminar seminar);
    Task DeleteSeminar(Seminar seminar);
    Task<Seminar> GetSeminarById(int id);
    Task<Seminar> GetSeminarBySubjectId(int subjectId);
    Task<IEnumerable<Seminar>> GetAllSeminars();
    Task<IEnumerable<Seminar>> GetAllSeminarsBySubject(int subjectId);
    IQueryable<Seminar> GetByCondition(Expression<Func<Seminar, bool>> expression);
}
