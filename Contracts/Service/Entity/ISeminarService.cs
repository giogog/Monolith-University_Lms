using Domain.Dtos;
using Domain.Models;

namespace Contracts;

public interface ISeminarService
{
    Task<Result<bool>> RegisterSeminar(int subjectId, SeminarDto seminar);
}
