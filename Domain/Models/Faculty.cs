namespace Domain.Models;

public class Faculty
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Places { get; set; }
    public ICollection<Student> Students { get; set; }
    public ICollection<Subject> Subjects { get; set; }
}
