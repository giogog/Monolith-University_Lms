using Domain.Models;

namespace Contracts;

public interface IAcademicService
{
    Task<double> CalculateStudentGpa(int studentId);
    Task<int> CalculateAchievedCredits(int studentId);
}
