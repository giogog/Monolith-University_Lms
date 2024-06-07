using Application.CQRS.Queries;
using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class GetSubjectHandler : IRequestHandler<GetSubjectQuery, Result<EnrollmentSubjectDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;

    public GetSubjectHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<Result<EnrollmentSubjectDto>> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
    {
        var teacherSubjects = await _repositoryManager.SubjectRepository
            .GetByCondition(t => t.Id == request.subjectId)
            .Include(t => t.Lectures)
            .Include(t => t.Seminars)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (teacherSubjects == null)
            return Result<EnrollmentSubjectDto>.Failed("NotFound", "Subject not found");

        if (!teacherSubjects.Lectures.Any())
            return Result<EnrollmentSubjectDto>.Failed("NotFound", "Lectures not found on this subject");

        var lectureTasks = teacherSubjects.Lectures.Select(async lecture =>
        {
            var takenPlaces = await _repositoryManager.EnrollmentRepository.GetAllEnrollmentByIds(e => e.LectureId == lecture.Id);
            var teacher = await _repositoryManager.TeacherRepository.GetByCondition(t => t.Id == lecture.TeacherId).Include(t => t.User).FirstOrDefaultAsync();
            return new EnrollmentLectureDto(lecture.Id,$"{teacher.User.Name} {teacher.User.Surname}", _mapper.Map<ScheduleTimesDto>(lecture.Schedule), lecture.LectureCapacity, takenPlaces.Count());
        });

        var seminarTasks = teacherSubjects.Seminars.Select(async seminar =>
        {
            var takenPlaces = await _repositoryManager.EnrollmentRepository.GetAllEnrollmentByIds(e => e.SeminarId == seminar.Id);
            var teacher = await _repositoryManager.TeacherRepository.GetByCondition(t => t.Id == seminar.TeacherId).Include(t => t.User).FirstOrDefaultAsync();
            return new EnrollmentSeminarDto(seminar.Id,$"{teacher.User.Name} {teacher.User.Surname}", _mapper.Map<ScheduleTimesDto>(seminar.Schedule), seminar.SeminarCapacity, takenPlaces.Count());
        });

        EnrollmentLectureDto[] enrollmentLectureDtos;
        EnrollmentSeminarDto[] enrollmentSeminarDtos;

        try
        {
            enrollmentLectureDtos = await Task.WhenAll(lectureTasks);
            enrollmentSeminarDtos = await Task.WhenAll(seminarTasks);
        }
        catch (Exception ex)
        {
            return Result<EnrollmentSubjectDto>.Failed("Error", ex.Message);
        }

        return Result<EnrollmentSubjectDto>.Success(new EnrollmentSubjectDto(teacherSubjects.Name, enrollmentLectureDtos, enrollmentSeminarDtos));
    }

}
