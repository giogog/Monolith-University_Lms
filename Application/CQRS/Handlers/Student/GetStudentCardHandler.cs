using Application.CQRS.Queries;
using Application.Services;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class GetStudentCardHandler : IRequestHandler<GetStudentCardQuery, Result<StudentCardDto>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager _serviceManager;

    public GetStudentCardHandler(IRepositoryManager repositoryManager,IServiceManager serviceManager)
    {
        _repositoryManager = repositoryManager;
        this._serviceManager = serviceManager;
    }
    public async Task<Result<StudentCardDto>> Handle(GetStudentCardQuery request, CancellationToken cancellationToken)
    {
        var student = await _repositoryManager.StudentRepository.GetByConditionAsync(s => s.User.UserName == request.personalId).FirstOrDefaultAsync();
        if (student == null)
            return Result<StudentCardDto>.Failed("NotFound","Student Not Found");

        var studentSubjects = await _serviceManager.SubjectService.GetSubjectsByStudentId(student.Id);
        if(!studentSubjects.Any())
            return Result<StudentCardDto>.Failed("NotFound", "Student Subjects not Found");
        var studentEnrollments = await _repositoryManager.EnrollmentRepository.GetByCondition(en => en.StudentId == student.Id).ToArrayAsync();
        if(!studentEnrollments.Any())
            return Result<StudentCardDto>.Failed("NotFound", "Student Enrollments not Found");

        var GPA = await _serviceManager.AcademicService.CalculateStudentGpa(student.Id);
        var PassedCredits = await _serviceManager.AcademicService.CalculateAchievedCredits(student.Id);

        if (studentEnrollments.Count() != studentSubjects.Count()) 
            return Result<StudentCardDto>.Failed("NotMatch","Enrollments doesn't match to subjects");

        List<StudentCardSubjectDto> studentCardSubjects = new List<StudentCardSubjectDto>();

        int index = 0;
        foreach (var studentSubject in studentSubjects)
        {
            studentCardSubjects.Add(new StudentCardSubjectDto(studentSubject.Name, _serviceManager.GradeService.GetGradeSystem(studentEnrollments[index].Grades, studentSubject.gradeTypes).Data, studentEnrollments[index].Mark));
        }


        return Result<StudentCardDto>.Success(new StudentCardDto(PassedCredits,GPA,studentCardSubjects));
    }
}
