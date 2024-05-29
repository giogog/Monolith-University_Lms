using Contracts;
using Domain.Dtos;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;

namespace Application.Services;

public class GradeService:IGradeService
{
    public Result<string[]> SetGradeSystem(string[] grades)
    {
        string[] gradeSystem = Enum.GetNames(typeof(GradeSystem));
        bool containsAll = grades.All(item => gradeSystem.Contains(item));

        if (!containsAll) 
            return Result<string[]>.Failed("GradeError", "Ivalid Grade Type");

        Array.Sort(grades);
        return Result<string[]>.Success(grades.ChangeRepeatedItemsNames());
    }
}
