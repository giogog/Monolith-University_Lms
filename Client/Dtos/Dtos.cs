namespace Client.Dtos;

public record RegisterDto
{
    public string PersonalID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Faculty { get; set; }
}

public record LoginResult
{
    public string Token { get; set; }
    public string Username { get; set; } // This property is not needed anymore
}

public class LoginDto
{
    public string PersonalId { get; set; }
    public string Password { get; set; }
}

//public record LoginDto
//{
//    public string PersonalID { get; set; }
//    public string Password { get; set; }
//}
public record LoginResponseDto(string PersonalId, string Token);

public record StudentModel
{
    public string SelectedFaculty { get; set; }
}

public record EnrollmentSubjectDto(string Name, EnrollmentLectureDto[] Lectures, EnrollmentSeminarDto[]? Seminars);
public record EnrollmentLectureDto(int lectureId, string TeacherFullName, ScheduleTimesDto ScheduleTimes, int Capacity, int TakenPlaces);
public record EnrollmentSeminarDto(int seminarId, string TeacherFullName, ScheduleTimesDto ScheduleTimes, int Capacity, int TakenPlaces);
public record ScheduleTimesDto(string DayOfWeek, TimeSpan StartTime, TimeSpan EndTime);
public record SubjectByFacultyDto(string Name, int subjectId);
public record SelectedSubjectDto(string SubjectName, string TeacherFullName);
public record EnrollToSubjectCommand(int SubjectId, int LectureId, int SeminarId, string PersonalId);
public record CapacityDto(int classId,int Capacity);