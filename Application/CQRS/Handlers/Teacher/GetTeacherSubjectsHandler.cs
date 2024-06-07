using Application.CQRS.Queries;
using AutoMapper;
using AutoMapper.Internal;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.CQRS.Handlers;

public class GetTeacherSubjectsHandler : IRequestHandler<GetTeacherSubjectsQuery, Result<IEnumerable<TeacherSubjectDto>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetTeacherSubjectsHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        this._mapper = mapper;
    }


    public async Task<Result<IEnumerable<TeacherSubjectDto>>> Handle(GetTeacherSubjectsQuery request, CancellationToken cancellationToken)
    {

        var teacherSubjects = await _repositoryManager.TeacherRepository
            .GetByCondition(t => t.User.UserName == request.personalId)
            .Include(t => t.Lectures)
                .ThenInclude(l => l.Subject)
            .Include(t => t.Seminars)
                .ThenInclude(s => s.Subject)
            .Select(t => new
            {
                // Project only the necessary fields
                Lectures = t.Lectures.Where(l=>l.IsActive).Select(l => new TeacherSubjectDto(t.Id, l.SubjectId, new TeacherScheduleDto(
                    l.Subject.Name,
                    nameof(Lecture),
                    _mapper.Map<ScheduleTimesDto>(l.Schedule)), l.Id)),
                Seminars = t.Seminars.Where(l => l.IsActive).Select(s => new TeacherSubjectDto(t.Id, s.SubjectId, new TeacherScheduleDto(
                    s.Subject.Name,
                    nameof(Seminar),
                    _mapper.Map<ScheduleTimesDto>(s.Schedule)), s.Id))
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (teacherSubjects == null)
            return Result<IEnumerable<TeacherSubjectDto>>.Failed("NotFound", "Teacher not Found");

        var subjectsList = teacherSubjects.Lectures.Concat(teacherSubjects.Seminars).ToList();

        return Result<IEnumerable<TeacherSubjectDto>>.Success(subjectsList);

    }
}
