using Domain.Dtos;
using Domain.Models;

namespace Contracts;

public interface IGradeService
{
    Result<string[]> SetGradeSystem(string[] grades);
    Task<Result<Dictionary<string, double>>> GetGradeSystem(double[] grades, int subjectId);
    Result<double[]> AddGradeToStudent(GradeDto gradeDto);
}
