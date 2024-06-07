using Application.CQRS.Commands;
using Contracts;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class LectureActivationHandler : IRequestHandler<LectureActivationCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;

    public LectureActivationHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<int>> Handle(LectureActivationCommand request, CancellationToken cancellationToken)
    {
        var lecture = await _repositoryManager.LectureRepository.GetLectureById(request.lectureId);
        if (lecture == null)
            return Result<int>.Failed("NotFound", "Lecture not found");
        lecture.IsActive = request.action;
        await _repositoryManager.LectureRepository.UpdateLecture(lecture);

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
