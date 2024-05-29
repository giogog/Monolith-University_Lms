using Application.CQRS.Commands;
using Contracts;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.CQRS.Handlers;

internal class SubjectActivationHandler : IRequestHandler<SubjectActivationCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;

    public SubjectActivationHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<int>> Handle(SubjectActivationCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repositoryManager.SubjectRepository.GetSubjectById(request.subjectId);
        if (subject == null)
            return Result<int>.Failed("NotFound", "Subject not found");

        subject.isActive = request.action;

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
