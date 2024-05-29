using Domain.Dtos;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.CQRS.Commands;

//Student
public record ApproveStudentApplicationCommand(string personalId) : IRequest<Result<string>>;
public record DeclineStudentApplicationCommand(string personalId) : IRequest<Result<string>>;

//Teacher
public record TeacherRegistrationCommand(string PersonalID, string Name, string Surname, 
    string Email, string Password, decimal Salary) : IRequest<IdentityResult>;


//Subject
public record SubjectRegistrationCommand(string Name, int CreditWeight, Semester Semester, string[] GradeTypes,
    string FacultyName, ICollection<LectureDto> Lectures, ICollection<SeminarDto>? Seminars):IRequest<Result<int>>;
public record SubjectUpdateCommand(int subjectId,string Name, int CreditWeight, Semester Semester, string[] GradeTypes) : IRequest<Result<int>>;
public record SubjectActivationCommand(int subjectId, bool action):IRequest<Result<int>>;

public record LectureAddCommand(int subjectId,int LectureCapacity, string TeacherPersonalId, DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime) : IRequest<Result<int>>;
public record SeminarAddCommand(int subjectId, int SeminarCapacity, string TeacherPersonalId, DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime) : IRequest<Result<int>>;
public record LectureDeleteCommand(int lectureId) : IRequest<Result<int>>;
public record SeminarDeleteCommand(int seminarId) : IRequest<Result<int>>;
