namespace Domain.Models;

public class University
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SemesterPayment { get; set; }
    public int CreditsToGraduate { get; set; }
    public int SubjectPayment { get; set; }
    public ICollection<Faculty> Faculties { get; set; }
}
