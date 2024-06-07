using Domain.Dtos;
using Domain.Models;

namespace Contracts;

public interface IGradeService
{
    Result<string[]> SetGradeSystem(string[] grades);
    Result<Dictionary<string, double>> GetGradeSystem(double[] grades, string[] gradeTypes);
    Result<double[]> AddGradeToStudent(GradeDto gradeDto);
}
