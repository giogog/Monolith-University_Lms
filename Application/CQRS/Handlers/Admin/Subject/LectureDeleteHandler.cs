using Application.CQRS.Commands;
using Contracts;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class LectureDeleteHandler : IRequestHandler<LectureDeleteCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;

    public LectureDeleteHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<int>> Handle(LectureDeleteCommand request, CancellationToken cancellationToken)
    {
        var lecture = await _repositoryManager.LectureRepository.GetLectureById(request.lectureId);
        if (lecture == null)
            return Result<int>.Failed("NotFound", "Lecture not found");
        await _repositoryManager.LectureRepository.DeleteLecture(lecture);

        try
        {
            var saveResult = await _repositoryManager.SaveAsync();
            return Result<int>.SuccesfullySaved(saveResult, saveResult);
        }
        catch (Exception ex)
        {
            return Result<int>.Failed("SavingError", ex.Message);
        }

    }
}
