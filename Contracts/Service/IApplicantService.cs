using Domain.Dtos;
using Domain.Models;

namespace Contracts;

public interface IApplicantService
{
    ApplicantDto CreateApplicant(User user);
}
