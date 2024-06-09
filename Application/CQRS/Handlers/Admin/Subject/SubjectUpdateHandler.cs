using Application.CQRS.Commands;
using Application.Services;
using Contracts;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class SubjectUpdateHandler : IRequestHandler<SubjectUpdateCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager _serviceManager;

    public SubjectUpdateHandler(IRepositoryManager repositoryManager, IServiceManager serviceManager)
    {
        _repositoryManager = repositoryManager;
        _serviceManager = serviceManager;
    }
    public async Task<Result<int>> Handle(SubjectUpdateCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repositoryManager.SubjectRepository.GetSubjectById(request.subjectId);
        if (subject == null)
            return Result<int>.Failed("NotFound","Subject not found");
        var gradesSortResult = _serviceManager.GradeService.SetGradeSystem(request.GradeTypes);
        if (!gradesSortResult.IsSuccess)
            return Result<int>.Failed(gradesSortResult.Code, gradesSortResult.Message);

        subject.Name = request.Name;
        subject.Semester = request.Semester;
        subject.Credits = request.CreditWeight;
        subject.gradeTypes = gradesSortResult.Data;

        await _repositoryManager.SubjectRepository.UpdateSubject(subject);

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
