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

//Addons
public record ScheduleTimesDto(string DayOfWeek, TimeSpan StartTime, TimeSpan EndTime);

//Subjects
public record SubjectDto(string Name, int Credits, Semester Semester, string[] gradeTypes,string FacultyName);
public record LectureDto(int LectureCapacity, string TeacherPersonalId, DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime);
//public record LectureDto(int LectureCapacity, string TeacherPersonalId, ScheduleTimesDto ScheduleTimes);
public record SeminarDto(int SeminarCapacity, string TeacherPersonalId, DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime);
//public record SeminarDto(int SeminarCapacity, string TeacherPersonalId, ScheduleTimesDto ScheduleTimes);
public record SubjectDtoGet(int id, string Name, int Credits, Semester Semester, string[] gradeTypes);

//Teacher
public record TeacherDto(string PersonalId,string Name, string Surname, string Email,decimal Salary);
public record TeacherScheduleDto(string SubjectName,string ClassType, ScheduleTimesDto ScheduleTimes);
public record TeacherSubjectDto(int teacherId, int subjectid, TeacherScheduleDto SubjectSchedule, int classId);
public record StudentEnrollmentDto(int EnrollmentId,string ClassType, string StudentPersonalId, string StudentFullName,Dictionary<string,double> Grades);
public record GradeDto(Dictionary<string, double> Grades, string GradeType, double Grade);

//Student
public record StudentDataDto(string Name, string Surname, string Email, decimal SemesterPay, AcademicStatus Status);
public record StudentCardSubjectDto(string Name, Dictionary<string,double> Grades, char Mark);
public record StudentCardDto(int FullCredits, double GPA, IEnumerable<StudentCardSubjectDto> Subjects);
public record ClassDto(string ClassType, ScheduleTimesDto time);
public record StudentScheduleDto(string SubjectName, IEnumerable<ClassDto> Classes);
public record SubjectByFacultyDto(string Name,int subjectId);
public record EnrollmentSubjectDto(string Name ,IEnumerable<EnrollmentLectureDto> Lectures, IEnumerable<EnrollmentSeminarDto>? Seminars);
public record EnrollmentLectureDto(int lectureId,string TeacherFullName, ScheduleTimesDto ScheduleTimes, int Capacity,int TakenPlaces);
public record EnrollmentSeminarDto(int seminarId, string TeacherFullName, ScheduleTimesDto ScheduleTimes, int Capacity, int TakenPlaces);
public record StudentActiveEnrollmentsDto(string SubjectName, string TeacherFullName);
public record CapacityDto(int classId, int Capacity);