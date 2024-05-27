using Application.CQRS.Commands;
using Application.SignalR;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.CQRS.Handlers;

public class SendNewApplicantDataHandler : IRequestHandler<SendNewApplicantDataCommand, Result<ApplicantDto>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager _serviceManager;
    private readonly IHubContext<ApplicantHub> _hubContext;

    public SendNewApplicantDataHandler(
        IRepositoryManager repositoryManager,
        IServiceManager serviceManager,
        IHubContext<ApplicantHub> hubContext)
    {
        _repositoryManager = repositoryManager;
        _serviceManager = serviceManager;
        _hubContext = hubContext;
    }
    public async Task<Result<ApplicantDto>> Handle(SendNewApplicantDataCommand request, CancellationToken cancellationToken)
    {
        var user = await _repositoryManager.UserRepository.GetApplicantWithExamResultsAsync(user => user.Id == request.userId);
        if (user == null)
            return Result<ApplicantDto>.Failed("NotFound", "User not found");
        var application = _serviceManager.ApplicantService.CreateApplicant(user);

        await _hubContext.Clients.Group("Admins").SendAsync("NewUser", application);

        return Result<ApplicantDto>.Success(application);
    }
}
