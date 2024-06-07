using Contracts;
using Domain.Enums;

namespace Application.Services;

public class StudentService : IStudentService
{
    public StudentService(IRepositoryManager repositoryManager)
    {
        
    }
    public Task SetStudentStatus(string personalId, AcademicStatus status)
    {
        throw new NotImplementedException();
    }
}
