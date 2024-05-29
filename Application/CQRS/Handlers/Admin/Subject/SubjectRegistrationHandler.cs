using Application.CQRS.Commands;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class SubjectRegistrationHandler : IRequestHandler<SubjectRegistrationCommand, Result<int>>
{
    private readonly IServiceManager _serviceManager;
    private readonly IRepositoryManager _repositoryManager;

    public SubjectRegistrationHandler(IServiceManager serviceManager, IRepositoryManager repositoryManager)
    {
        _serviceManager = serviceManager;
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<int>> Handle(SubjectRegistrationCommand request, CancellationToken cancellationToken)
    {
        var gradesSortResult = _serviceManager.GradeService.SetGradeSystem(request.GradeTypes);
        if (!gradesSortResult.IsSuccess)
            return Result<int>.Failed(gradesSortResult.Code, gradesSortResult.Message);

        var subjectDto = new SubjectDto(request.Name, request.CreditWeight, request.Semester, gradesSortResult.Data, request.FacultyName);
        
        var subjectRegisterResult = await _serviceManager.SubjectService.RegisterNewSubject(subjectDto); 
        if (!subjectRegisterResult.IsSuccess)
            return Result<int>.Failed(subjectRegisterResult.Code, subjectRegisterResult.Message);

        var subject = await _repositoryManager.SubjectRepository.GetByCondition(s=>s.Name==request.Name).FirstOrDefaultAsync();
        foreach(var lecture in request.Lectures)
        {
            var LectureRegistrationResult = await _serviceManager.LectureService.RegisterLecture(subject.Id,lecture);
            if (!LectureRegistrationResult.IsSuccess) 
            {
                //LOG data
            }

        }
        foreach (var seminar in request.Seminars)
        {
            var SeminarRegistrationResult = await _serviceManager.SeminarService.RegisterSeminar(subject.Id, seminar);
            if (!SeminarRegistrationResult.IsSuccess)
            {
                //LOG data
            }

        }

        try
        {
            var saveResult = await _repositoryManager.SaveAsync();
            return Result<int>.SuccesfullySaved(saveResult, saveResult);
        }
        catch (Exception ex)
        {
            return Result<int>.Failed("SavingError",ex.Message);
        }
    }
}
