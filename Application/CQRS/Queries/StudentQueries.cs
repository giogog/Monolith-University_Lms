using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CQRS.Queries;

public record GetStudentDataQuery(string personalId):IRequest<Result<StudentDataDto>>;
public record GetStudentCardQuery(string personalId):IRequest<Result<StudentCardDto>>;
public record GetStudentScheduleQuery(string personalId):IRequest<Result<IEnumerable<StudentScheduleDto>>>;
public record GetSubjectsByFacultyQuery(string Faculty):IRequest<Result<IEnumerable<SubjectByFacultyDto>>>;
public record GetSubjectQuery(int subjectId):IRequest<Result<EnrollmentSubjectDto>>;
