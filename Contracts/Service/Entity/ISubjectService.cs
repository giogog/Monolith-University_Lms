using Domain.Dtos;
using Domain.Models;

namespace Contracts;

public interface ISubjectService
{
    Task<Result<int>> RegisterNewSubject(SubjectDto subjectdto);
}
