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
