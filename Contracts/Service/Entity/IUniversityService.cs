using Domain.Models;

namespace Contracts;

public interface IUniversityService
{
    Task<Result<IEnumerable<string>>> GetFacultyNames();

}
