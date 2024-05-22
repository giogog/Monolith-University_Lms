using Domain.Models;

namespace Contracts;

public interface IUniversityRepository
{
    Task UpdateUniversityAsync(University university);
    Task<University> GetUniversityAsync();
}
