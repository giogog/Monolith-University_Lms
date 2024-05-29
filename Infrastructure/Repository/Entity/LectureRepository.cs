using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class LectureRepository(DomainDataContext context) : BaseRepository<Lecture>(context), ILectureRepository
{

    public async Task AddLecture(Lecture lecture) => Create(lecture);

    public async Task DeleteLecture(Lecture lecture) => Delete(lecture);

    public async Task UpdateLecture(Lecture lecture) => Update(lecture);

    public async Task<Lecture> GetLectureById(int id) => 
        await FindByCondition(l => l.Id == id)
        .Include(l=>l.StudentEnrollments)
        .FirstOrDefaultAsync();

    public async Task<IEnumerable<Lecture>> GetAllLectures() =>
    await FindAll()
    .Include(l => l.StudentEnrollments)
    .ToArrayAsync();

    public async Task<Lecture> GetLectureBySubjectId(int subjectId) => 
        await FindByCondition(l => l.SubjectId == subjectId)
                .Include(l => l.StudentEnrollments)
        .FirstOrDefaultAsync();

    public async Task<IEnumerable<Lecture>> GetAllLectureByTeacherId(int teacherId) =>
    await FindByCondition(l => l.TeacherId == teacherId)
            .Include(l => l.StudentEnrollments)
    .ToArrayAsync();
    public async Task<IEnumerable<Lecture>> GetAllLecturesBySubject(int subjectId) =>
    await FindByCondition(l => l.SubjectId == subjectId)
    .Include(l => l.StudentEnrollments)
    .ToArrayAsync();
    public IQueryable<Lecture> GetByCondition(Expression<Func<Lecture, bool>> expression) => FindByCondition(expression);

}
