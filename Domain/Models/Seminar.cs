using Domain.Props;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Seminar
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }

    [Range(0, 300)]
    public int SeminarCapacity { get; set; }
    public ScheduleProperty Schedule { get; set; }
    public int TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    public ICollection<StudentEnrollment>? StudentEnrollments { get; set; }
}
