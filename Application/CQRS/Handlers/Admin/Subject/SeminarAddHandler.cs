using Application.CQRS.Commands;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class SeminarAddHandler : IRequestHandler<SeminarAddCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager _serviceManager;

    public SeminarAddHandler(IRepositoryManager repositoryManager,IServiceManager serviceManager)
    {
        _repositoryManager = repositoryManager;
        _serviceManager = serviceManager;
    }
    public async Task<Result<int>> Handle(SeminarAddCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repositoryManager.SubjectRepository.GetSubjectById(request.subjectId);
        if (subject == null)
            return Result<int>.Failed("NotFound","Subject not found");

        var seminarDto = new SeminarDto(request.SeminarCapacity,request.TeacherPersonalId
            ,request.DayOfWeek,request.StartTime,request.EndTime);

        var seminarAddResult = await _serviceManager.SeminarService.RegisterSeminar(subject.Id, seminarDto);

        if (!seminarAddResult.IsSuccess)
            return Result<int>.Failed(seminarAddResult.Code, seminarAddResult.Message);

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
