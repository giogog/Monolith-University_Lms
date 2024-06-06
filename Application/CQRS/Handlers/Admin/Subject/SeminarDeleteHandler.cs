using Application.CQRS.Commands;
using Contracts;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class SeminarDeleteHandler : IRequestHandler<SeminarDeleteCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;

    public SeminarDeleteHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<int>> Handle(SeminarDeleteCommand request, CancellationToken cancellationToken)
    {
        var seminar = await _repositoryManager.SeminarRepository.GetSeminarById(request.seminarId);
        if (seminar == null)
            return Result<int>.Failed("NotFound", "Lecture not found");
        await _repositoryManager.SeminarRepository.DeleteSeminar(seminar);

        try
        {
            var saveResult = await _repositoryManager.SaveAsync();
            return Result<int>.Success(saveResult);
        }
        catch (Exception ex)
        {
            return Result<int>.Failed("SavingError", ex.Message);
        }

    }
}