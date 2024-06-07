using Domain.Dtos;
using Domain.Models;

namespace Contracts;

public interface ISubjectService
{
    Task<Result<int>> RegisterNewSubject(SubjectDto subjectdto);
    Task<IEnumerable<Subject>> GetSubjectsByStudentId(int studentId);
    Task<IEnumerable<Subject>> GetActiveSubjectsByStudentId(int studentId);
    Task<IEnumerable<Subject>> GetPassedSubjectsByStudentId(int studentId);
}
