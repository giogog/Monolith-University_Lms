using Application.CQRS.Queries;
using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class GetStudentScheduleHandler : IRequestHandler<GetStudentScheduleQuery, Result<IEnumerable<StudentScheduleDto>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager _serviceManager;
    private readonly IMapper _mapper;
    public GetStudentScheduleHandler(IRepositoryManager repositoryManager, IServiceManager serviceManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _serviceManager = serviceManager;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<StudentScheduleDto>>> Handle(GetStudentScheduleQuery request, CancellationToken cancellationToken)
    {
        var student = await _repositoryManager.StudentRepository.GetByConditionAsync(s => s.User.UserName == request.personalId).FirstOrDefaultAsync();
        if(student == null)
            return Result<IEnumerable<StudentScheduleDto>>.Failed("NotFound", "Student not Found");

        var activeEnrollmentsByStudent = await _repositoryManager.EnrollmentRepository
            .GetByCondition(en => en.StudentId == student.Id && en.IsActive)
            .Include(en => en.Lecture)
            .Include(en => en.Seminar)
            .ToArrayAsync();

        if (!activeEnrollmentsByStudent.Any())
            return Result<IEnumerable<StudentScheduleDto>>.Failed("NotFound", "Active enrollments not Found");

        var activeSubjectsbyStudent = await _serviceManager.SubjectService.GetActiveSubjectsByStudentId(student.Id);
        if (!activeSubjectsbyStudent.Any())
            return Result<IEnumerable<StudentScheduleDto>>.Failed("NotFound", "Active Subjects not Found");


        if (activeSubjectsbyStudent.Count() != activeEnrollmentsByStudent.Count())
            return Result<IEnumerable<StudentScheduleDto>>.Failed("NotMatch","Enrollments doesn't match to active SubjectsbyStudent");

        var schedules = new List<StudentScheduleDto>();


        foreach (var subject in activeSubjectsbyStudent)
        {

            var activities = new List<ClassDto>();
            var enrollment = activeEnrollmentsByStudent.Where(en => en.Lecture.SubjectId == subject.Id).FirstOrDefault();
            if (enrollment == null)
                continue;
            var LecturetimeDto = _mapper.Map<ScheduleTimesDto>(enrollment.Lecture.Schedule);
            activities.Add(new ClassDto("Lecture", LecturetimeDto));
            if (enrollment.Seminar != null)
            {
                var SeminartimeDto = _mapper.Map<ScheduleTimesDto>(enrollment.Seminar.Schedule);
                activities.Add(new ClassDto("Seminar", SeminartimeDto));
            }
            schedules.Add(new StudentScheduleDto(subject.Name, activities));
        }

        return Result<IEnumerable<StudentScheduleDto>>.Success(schedules);
    }
}
