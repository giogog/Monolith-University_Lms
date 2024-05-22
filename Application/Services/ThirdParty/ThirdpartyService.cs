using Contracts;
using Domain.Dtos;

namespace Application.Services;

public class ThirdpartyService : IThirdpartyService
{

    public async Task<ExamsCardDto> GenerateRandomResultData()
    {
        string[] mandatorySubjects = new string[] { "Middle Earth Language", "Elven", "Magic" };
        string[] subjectsToChoose = new string[] { "Math", "History" };
        Random random = new Random();
        var results = new List<ExamResultsDto>
        {
            new ExamResultsDto(mandatorySubjects[0],random.Next(30, 101)),
            new ExamResultsDto(mandatorySubjects[1],random.Next(30, 101)),
            new ExamResultsDto(mandatorySubjects[2],random.Next(30, 101)),
            new ExamResultsDto(subjectsToChoose[random.Next(0, 2)],random.Next(30, 101))
        };
        return new ExamsCardDto(0.5,results);
    }

}
