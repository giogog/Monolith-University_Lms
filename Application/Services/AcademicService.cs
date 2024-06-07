using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AcademicService : IAcademicService
{
    private readonly IRepositoryManager _repositoryManager;

    public AcademicService(IRepositoryManager repositoryManager)
    {
        this._repositoryManager = repositoryManager;
    }

    public async Task<int> CalculateAchievedCredits(int studentId)
    {
        var studentEnrollments = await _repositoryManager.EnrollmentRepository
            .GetByCondition(en => en.StudentId == studentId)
            .Include(en => en.Lecture).ThenInclude(l => l.Subject).ToArrayAsync();

        if (!studentEnrollments.Any()) return 0;

        var PassedSubjects = studentEnrollments.Select(en => en.Lecture.Subject);

        if (PassedSubjects.Any()) return 0;

        return PassedSubjects.Sum(s => s.Credits);
    }

    public async Task<double> CalculateStudentGpa(int studentId)
    {
        var PassedEnrollments = await _repositoryManager.EnrollmentRepository
            .GetByCondition(en => en.StudentId == studentId && en.IsPassed).ToListAsync();

        var AllPassedSubjectsSummedGrade = PassedEnrollments.Sum(en => en.FullGrade);
        var NumOfPassedSubjects = PassedEnrollments.Count();
        if (AllPassedSubjectsSummedGrade == 0)
            return 0;

        try
        {
            double GPA = AllPassedSubjectsSummedGrade / NumOfPassedSubjects;
            return GPA;
        }
        catch (Exception ex)
        {
            return 0;
        }

    }
}
