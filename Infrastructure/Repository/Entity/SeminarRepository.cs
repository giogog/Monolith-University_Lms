using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class SeminarRepository(DomainDataContext context) : BaseRepository<Seminar>(context), ISeminarRepository
{

    public async Task AddSeminar(Seminar seminar) => Create(seminar);

    public async Task DeleteSeminar(Seminar seminar) => Delete(seminar);

    public async Task UpdateSeminar(Seminar seminar) => Update(seminar);

    public async Task<IEnumerable<Seminar>> GetAllSeminars() => 
        await FindAll().Include(l => l.StudentEnrollments)
        .ToArrayAsync();

    public async Task<IEnumerable<Seminar>> GetAllSeminarsBySubject(int subjectId) => 
        await FindByCondition(s=>s.SubjectId == subjectId).Include(l => l.StudentEnrollments)
        .ToArrayAsync();

    public async Task<Seminar> GetSeminarById(int id) => 
        await FindByCondition(s => s.Id == id)
        .Include(l => l.StudentEnrollments)
        .FirstOrDefaultAsync();

    public async Task<Seminar> GetSeminarBySubjectId(int subjectId) => 
        await FindByCondition(s => s.SubjectId == subjectId)
        .Include(l => l.StudentEnrollments)
        .FirstOrDefaultAsync();
    public IQueryable<Seminar> GetByCondition(Expression<Func<Seminar, bool>> expression) => FindByCondition(expression);

}
