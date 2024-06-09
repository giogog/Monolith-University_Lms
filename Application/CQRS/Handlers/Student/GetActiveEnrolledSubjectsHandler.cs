using Application.CQRS.Queries;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.CQRS.Handlers;

public class GetActiveEnrolledSubjectsHandler : IRequestHandler<GetActiveEnrolledSubjectsQuery, Result<IEnumerable<StudentActiveEnrollmentsDto>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager serviceManager;

    public GetActiveEnrolledSubjectsHandler(IRepositoryManager repositoryManager, IServiceManager serviceManager)
    {
        _repositoryManager = repositoryManager;
        this.serviceManager = serviceManager;
    }
    public async Task<Result<IEnumerable<StudentActiveEnrollmentsDto>>> Handle(GetActiveEnrolledSubjectsQuery request, CancellationToken cancellationToken)
    {
        var student = await _repositoryManager.StudentRepository.GetByConditionAsync(s => s.User.UserName == request.personalId).FirstOrDefaultAsync();
        if (student == null)
        {
            return Result<IEnumerable<StudentActiveEnrollmentsDto>>.Failed("Not Found", "Student not found");
        }

        var subjects = await serviceManager.SubjectService.GetActiveSubjectsByStudentId(student.Id);
        if (!subjects.Any())
        {
            return Result<IEnumerable<StudentActiveEnrollmentsDto>>.Failed("Not Found", "No active subjects found for the student");
        }

        var activeEnrollments = await _repositoryManager.EnrollmentRepository.GetByCondition(en => en.StudentId == student.Id && en.IsActive)
            .Include(en => en.Lecture)
                .ThenInclude(l => l.Teacher)
                    .ThenInclude(t => t.User)
            .ToListAsync();

        List<StudentActiveEnrollmentsDto> enrollmentsDto = subjects
            .Select(subject =>
            {
                var enrollment = activeEnrollments.FirstOrDefault(en => en.Lecture.SubjectId == subject.Id);
                var teacherName = enrollment != null ? $"{enrollment.Lecture.Teacher.User.Name} {enrollment.Lecture.Teacher.User.Surname}" : "No teacher assigned";
                return new StudentActiveEnrollmentsDto(subject.Name, teacherName);
            })
            .ToList();

        return Result<IEnumerable<StudentActiveEnrollmentsDto>>.Success(enrollmentsDto);
    }

}
