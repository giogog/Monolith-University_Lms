using Domain.Enums;

namespace Domain.Models;

public class Subject
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public int Credits { get; set; }
    public Semester Semester { get; set; }
    public string[] gradeTypes { get; set; }
    public bool isActive {  get; set; }
    public int FacultyId { get; set; }
    public Faculty Faculty { get; set; }
    public ICollection<Lecture> Lectures { get; set; }
    public ICollection<Seminar>? Seminars { get; set; }
}
