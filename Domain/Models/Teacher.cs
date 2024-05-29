using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Teacher
{
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, 10000.00, ErrorMessage = "Salary must be between $0.01 and $10,000.00")]
    public decimal Salary { get; set; }
    public User User { get; set; }
    public ICollection<Lecture> Lectures { get; set; }
    public ICollection<Seminar> Seminars { get; set; }
}
