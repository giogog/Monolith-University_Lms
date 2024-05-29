using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using Domain.Props;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class SeminarService : ISeminarService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public SeminarService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public async Task<Result<bool>> RegisterSeminar(int subjectId,SeminarDto seminarDto)
    {
        var teacher = await _repositoryManager.TeacherRepository.GetByCondition(t => t.User.UserName == seminarDto.TeacherPersonalId).FirstOrDefaultAsync();
        if (teacher == null)
            return Result<bool>.Failed("NotFound","Teacher Not found");

        var seminar = new Seminar
        {
            SubjectId = subjectId,
            Schedule = new ScheduleProperty { DayOfWeek = seminarDto.DayOfWeek, StartTime = seminarDto.StartTime, EndTime = seminarDto.EndTime },
            SeminarCapacity = seminarDto.SeminarCapacity,
            TeacherId = teacher.Id
        };

        await _repositoryManager.SeminarRepository.AddSeminar(seminar);

        return Result<bool>.Success(true);
    }
}
