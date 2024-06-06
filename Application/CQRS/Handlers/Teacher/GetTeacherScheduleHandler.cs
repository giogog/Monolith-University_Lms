using Application.CQRS.Queries;
using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.CQRS.Handlers;

public class GetTeacherScheduleHandler : IRequestHandler<GetTeacherScheduleQuery, Result<IEnumerable<TeacherScheduleDto>>>
{
    private readonly IServiceManager _serviceManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetTeacherScheduleHandler(IServiceManager serviceManager, IRepositoryManager repositoryManager,IMapper mapper)
    {
        _serviceManager = serviceManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<TeacherScheduleDto>>> Handle(GetTeacherScheduleQuery request, CancellationToken cancellationToken)
    {
        var teacherSchedule = await _repositoryManager.TeacherRepository
            .GetByCondition(t => t.User.UserName == request.personalId)
            .Include(t => t.Lectures)
            .ThenInclude(l => l.Subject)
            .Include(t => t.Seminars)
            .ThenInclude(s => s.Subject)
            .Select(t => new
            {
                // Project only the necessary fields
                Lectures = t.Lectures.Select(l => new TeacherScheduleDto(
                    l.Subject.Name,
                    nameof(Lecture),
                    _mapper.Map<ScheduleTimesDto>(l.Schedule))),
                Seminars = t.Seminars.Select(s => new TeacherScheduleDto(
                    s.Subject.Name,
                    nameof(Seminar),
                    _mapper.Map<ScheduleTimesDto>(s.Schedule)))
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (teacherSchedule == null)
            return Result<IEnumerable<TeacherScheduleDto>>.Failed("NotFound", "Teacher not Found");

        var scheduleList = teacherSchedule.Lectures.Concat(teacherSchedule.Seminars).ToList();

        return Result<IEnumerable<TeacherScheduleDto>>.Success(scheduleList);
    }

}
