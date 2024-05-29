using Domain.Models;

namespace Contracts;

public interface IGradeService
{
    Result<string[]> SetGradeSystem(string[] grades);
}
