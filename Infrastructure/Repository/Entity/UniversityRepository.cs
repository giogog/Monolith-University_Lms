using Contracts;
using Domain.Models;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UniversityRepository(DomainDataContext context) : BaseRepository<University>(context), IUniversityRepository
{
    public async Task<University> GetUniversityAsync() => await FindAll().FirstOrDefaultAsync();

    public async Task UpdateUniversityAsync(University university) => Update(university);
}
