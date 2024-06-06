using Domain.Dtos;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Queries;

public record GetTeacherDataQuery(string personalId) : IRequest<Result<TeacherDto>>;
public record GetTeacherScheduleQuery(string personalId) : IRequest<Result<IEnumerable<TeacherScheduleDto>>>;
public record GetTeacherSubjectsQuery(string personalId) : IRequest<Result<IEnumerable<TeacherSubjectDto>>>;
public record GetLectureEnrollmentsQuery(int lectureId):IRequest<Result<IEnumerable<StudentEnrollmentDto>>>;
public record GetSeminarEnrollmentsQuery(int seminarId) : IRequest<Result<IEnumerable<StudentEnrollmentDto>>>;

