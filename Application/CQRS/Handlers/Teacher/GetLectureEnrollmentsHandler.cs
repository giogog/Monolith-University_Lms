using Application.CQRS.Queries;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.CQRS.Handlers;

public class GetLectureEnrollmentsHandler : IRequestHandler<GetLectureEnrollmentsQuery, Result<IEnumerable<StudentEnrollmentDto>>>
{
    private readonly IServiceManager _serviceManager;
    private readonly IRepositoryManager _repositoryManager;

    public GetLectureEnrollmentsHandler(IServiceManager serviceManager,IRepositoryManager repositoryManager)
    {
        _serviceManager = serviceManager;
        _repositoryManager = repositoryManager;
    }
    //public async Task<Result<IEnumerable<StudentEnrollmentDto>>> Handle(GetLectureEnrollmentsQuery request, CancellationToken cancellationToken)
    //{
    //    var enrollments = await _repositoryManager.EnrollmentRepository.GetByCondition(en=>en.LectureId==request.lectureId)
    //        .Include(en=>en.Student)
    //        .ThenInclude(s=>s.User)
    //        .ToListAsync();
    //    if (!enrollments.Any())
    //        return Result<IEnumerable<StudentEnrollmentDto>>.Failed("NotFound", "Enrollments not found");

    //    StudentEnrollmentDto[] enrollmentDtos = new StudentEnrollmentDto[enrollments.Count()];
    //    for (int i = 0; i < enrollments.Count(); i++)
    //    {
    //        var gradeSystem = await _serviceManager.GradeService.GetGradeSystem(enrollments[i].Grades, request.subject);
    //        if (gradeSystem == null)
    //            return Result<IEnumerable<StudentEnrollmentDto>>.Failed(gradeSystem.Code, gradeSystem.Message);
    //        enrollmentDtos[i] = new StudentEnrollmentDto(enrollments[i].Id, 
    //                                                    nameof(Lecture), enrollments[i].Student.User.UserName , 
    //                                                    $"{enrollments[i].Student.User.Name} {enrollments[i].Student.User.Surname}",
    //                                                    gradeSystem.Data);

    //    }

    //    return Result<IEnumerable<StudentEnrollmentDto>>.Success(enrollmentDtos);

    //}

    public async Task<Result<IEnumerable<StudentEnrollmentDto>>> Handle(GetLectureEnrollmentsQuery request, CancellationToken cancellationToken)
    {

        var enrollments = await _repositoryManager.EnrollmentRepository.GetByCondition(en => en.LectureId == request.lectureId)
            .Include(en => en.Student)
            .ThenInclude(s => s.User)
            .ToListAsync(cancellationToken);

        if (!enrollments.Any())
            return Result<IEnumerable<StudentEnrollmentDto>>.Failed("NotFound", "Enrollments not found");

        var subject = _repositoryManager.LectureRepository.GetByCondition(l=>l.Id == request.lectureId).Select(l=>l.Subject).FirstOrDefault();

        var tasks = enrollments.Select(async enrollment =>
        {
            var gradeSystem =  _serviceManager.GradeService.GetGradeSystem(enrollment.Grades, subject.gradeTypes);
            if (!gradeSystem.IsSuccess)
                throw new InvalidOperationException("Grade system not found.");

            return new StudentEnrollmentDto(
                enrollment.Id,
                nameof(Lecture),
                enrollment.Student.User.UserName,
                $"{enrollment.Student.User.Name} {enrollment.Student.User.Surname}",
                gradeSystem.Data);
        });

        StudentEnrollmentDto[] enrollmentDtos;
        try
        {
            enrollmentDtos = await Task.WhenAll(tasks);
        }
        catch (InvalidOperationException ex)
        {
            return Result<IEnumerable<StudentEnrollmentDto>>.Failed("Error", ex.Message);
        }

        return Result<IEnumerable<StudentEnrollmentDto>>.Success(enrollmentDtos);
    }

}
