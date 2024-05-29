using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using Domain.Props;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class LectureService : ILectureService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    public LectureService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<Result<bool>> RegisterLecture(int subjectId,LectureDto lectureDto)
    {
        var teacher = await _repositoryManager.TeacherRepository.GetByCondition(t => t.User.UserName == lectureDto.TeacherPersonalId).FirstOrDefaultAsync();
        if (teacher == null)
            return Result<bool>.Failed("NotFound", "Teacher Not found");

        var lecture = new Lecture
        {
            SubjectId = subjectId,
            Schedule = new ScheduleProperty { DayOfWeek = lectureDto.DayOfWeek, StartTime = lectureDto.StartTime, EndTime = lectureDto.EndTime },
            LectureCapacity = lectureDto.LectureCapacity,
            TeacherId = teacher.Id
        };

        await _repositoryManager.LectureRepository.AddLecture(lecture);

        return Result<bool>.Success(true);
    }
}
