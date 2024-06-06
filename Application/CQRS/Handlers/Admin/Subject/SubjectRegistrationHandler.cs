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
        //Set up Grade System
        var gradesSortResult = _serviceManager.GradeService.SetGradeSystem(request.GradeTypes);
        if (!gradesSortResult.IsSuccess)
            return Result<int>.Failed(gradesSortResult.Code, gradesSortResult.Message);

        //Register Subject First
        var subjectDto = new SubjectDto(request.Name, request.CreditWeight, request.Semester, gradesSortResult.Data, request.FacultyName);
        
        var subjectRegisterResult = await _serviceManager.SubjectService.RegisterNewSubject(subjectDto); 
        if (!subjectRegisterResult.IsSuccess)
            return Result<int>.Failed(subjectRegisterResult.Code, subjectRegisterResult.Message);

        //Register Seminars and Lectures seperately after subject registrations
        var failedRegistrations = new List<string>();
        var subject = await _repositoryManager.SubjectRepository.GetByCondition(s=>s.Name==request.Name).FirstOrDefaultAsync();
        foreach (var lecture in request.Lectures)
        {
            var lectureRegistrationResult = await _serviceManager.LectureService.RegisterLecture(subject.Id, lecture);
            if (!lectureRegistrationResult.IsSuccess)
            {
                failedRegistrations.Add($"Lecture failed: {lectureRegistrationResult.Message}");
            }
        }
        foreach (var seminar in request.Seminars)
        {
            var seminarRegistrationResult = await _serviceManager.SeminarService.RegisterSeminar(subject.Id, seminar);
            if (!seminarRegistrationResult.IsSuccess)
            {
                failedRegistrations.Add($"Seminar failed: {seminarRegistrationResult.Message}");
            }
        }


        //Check if seminar or lecture registrations failed
        if (failedRegistrations.Count < request.Lectures.Count + request.Seminars.Count)
        {
            try
            {
                var saveResult = await _repositoryManager.SaveAsync();
                return Result<int>.Success(saveResult);
            }
            catch (Exception ex)
            {

                failedRegistrations.Add($"Saving error: {ex.Message}");
                return Result<int>.Failed("SavingError", string.Join("\n", failedRegistrations));
            }
        }
        else
        {
            return Result<int>.Failed("RegistrationError", string.Join("\n", failedRegistrations));
        }

    }
}
