using Domain.CQRS.Queries;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Handlers;

public class GetApplicantsHandler : IRequestHandler<GetApplicantsQuery, Result<IEnumerable<ApplicantDto>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager _serviceManager;

    public GetApplicantsHandler(IRepositoryManager repositoryManager, IServiceManager serviceManager)
    {
        _repositoryManager = repositoryManager;
        _serviceManager = serviceManager;
    }

    public async Task<Result<IEnumerable<ApplicantDto>>> Handle(GetApplicantsQuery request, CancellationToken cancellationToken)
    {
        var role = await _repositoryManager.RoleRepository.GetRoleByNameAsync("Applicant");
        var applicants = await _repositoryManager.UserRoleRepository.GetApplicantsAsync(role.Id);
        if (applicants.Count() == 0)
            return Result<IEnumerable<ApplicantDto>>.Failed("NotFound", "There are not applicants");

        List<ApplicantDto> applicantDtos = new List<ApplicantDto>();

        foreach(var applicant in applicants)
        {
            
            applicantDtos.Add(_serviceManager.ApplicantService.CreateApplicant(applicant));
        }

        return Result<IEnumerable<ApplicantDto>>.Success(applicantDtos);
    }
}
