using Domain.CQRS.Queries;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;

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
        var applicants = await _repositoryManager.UserRoleRepository.GetUsersByRoleAsync(-2);
    }
}
