using Contracts;
using Domain.Models;

namespace Application.Services;

public class UniversityService : IUniversityService
{
    private readonly IRepositoryManager _repositoryManager;

    public UniversityService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<IEnumerable<string>>> GetFacultyNames()
    {
        var faculties = await _repositoryManager.FacultyRepository.GetAllFacultiesAsync();

        if (faculties.Count() == null)
            return Result<IEnumerable<string>>.Failed("NotFound", "There are no Faculties");

        List<string> facultyNames = new List<string>();

        foreach (var faculty in faculties)
        {
            facultyNames.Add(faculty.Name);
        }
        return Result<IEnumerable<string>>.Success(facultyNames);
    }


}
