using Domain.Enums;
using Domain.Models;

namespace Domain.Dtos;


//Third Party 
public record ExamResultsDto(string Subject, int Grade);
public record ExamsCardDto(double Grant, IEnumerable<ExamResultsDto> Results);

//User
public record LoginDto(string PersonalId, string Password);
public record LoginResponseDto(string PersonalId, string Token);
public record RegisterDto(string PersonalID, string Name, string Surname, string Email, string Password, string Faculty, AcademicRole Role,ExamResults? ExamResults);

//Applicants
public record ApplicantDto(string Name, string Surname, string PersonalId, string Faculty, ExamsCardDto examCard);

//Subjects
public record SubjectDto(string Name, int Credits, Semester Semester, string[] gradeTypes,string FacultyName);
public record LectureDto(int LectureCapacity, string TeacherPersonalId, DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime);
public record SeminarDto(int SeminarCapacity, string TeacherPersonalId, DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime);

public record SubjectDtoGet(int id, string Name, int Credits, Semester Semester, string[] gradeTypes);

