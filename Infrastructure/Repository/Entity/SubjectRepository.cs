using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class SubjectRepository(DomainDataContext context) : BaseRepository<Subject>(context), ISubjectRepository
{
    public async Task AddSubject(Subject subject) => Create(subject);

    public async Task DeleteSubject(Subject subject) => Delete(subject);

    public async Task UpdateSubject(Subject subject) => Update(subject);
    public async Task<IEnumerable<Subject>> GetAllSubjects() => 
        await FindAll().ToArrayAsync();

    public async Task<Subject> GetSubjectById(int id) =>   
        await FindByCondition(s => s.Id == id)
        .FirstOrDefaultAsync();

    public IQueryable<Subject> GetByCondition(Expression<Func<Subject, bool>> expression) => FindByCondition(expression);


}
