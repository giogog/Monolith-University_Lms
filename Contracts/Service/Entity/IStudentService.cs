using Domain.Enums;

namespace Contracts;

public interface IStudentService
{
    Task SetStudentStatus(string personalId, AcademicStatus status);
}
