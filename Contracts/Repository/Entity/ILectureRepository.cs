using Domain.Models;
using System.Linq.Expressions;

namespace Contracts;

public interface ILectureRepository
{
    Task AddLecture(Lecture lecture);
    Task UpdateLecture(Lecture lecture);
    Task DeleteLecture(Lecture lecture);
    Task<Lecture> GetLectureById(int id);
    Task<IEnumerable<Lecture>> GetAllLectureByTeacherId(int teacherId);
    Task<IEnumerable<Lecture>> GetAllLectures();
    Task<IEnumerable<Lecture>> GetAllLecturesBySubject(int subjectId);
    IQueryable<Lecture> GetByCondition(Expression<Func<Lecture, bool>> expression);

}
