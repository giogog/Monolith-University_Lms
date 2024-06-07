using Application.CQRS.Commands;
using Contracts;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class SeminarActivationHandler : IRequestHandler<SeminarActivationCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;

    public SeminarActivationHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<int>> Handle(SeminarActivationCommand request, CancellationToken cancellationToken)
    {
        var seminar = await _repositoryManager.SeminarRepository.GetSeminarById(request.seminarId);
        if (seminar == null)
            return Result<int>.Failed("NotFound", "Lecture not found");
        seminar.IsActive = request.action;
        await _repositoryManager.SeminarRepository.UpdateSeminar(seminar);

        try
        {
            var saveResult = await _repositoryManager.SaveAsync();
            return Result<int>.SuccesfullyUpdated(saveResult, saveResult);
        }
        catch (Exception ex)
        {
            return Result<int>.Failed("SavingError", ex.Message);
        }

    }
}