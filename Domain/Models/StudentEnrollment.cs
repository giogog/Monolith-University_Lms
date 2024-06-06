using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class StudentEnrollment
{
    public int Id { get; set; }
    public int LectureId { get; set; }
    public Lecture? Lecture { get; set; }
    public int? SeminarId { get; set; }
    public Seminar? Seminar { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }

    [AllowedValues('A', 'B', 'C', 'D', 'E', 'F')]
    public char Mark { get; set; } = 'F';
    public double[] Grades { get; set; }
    public double FullGrade => Grades?.Sum() ?? 0;
    public bool IsPassed { get; set; }
    public bool IsActive { get; set; }

    public StudentEnrollment()
    {
        
    }

    public StudentEnrollment(int size)
    {
        Grades = new double[size];
    }
}
