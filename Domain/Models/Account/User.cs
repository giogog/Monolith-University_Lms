using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class User:IdentityUser<int>
{
    [RegularExpression(@"^5\d{8}$", ErrorMessage = "Phone number must start with 5 and must be 9 digits long.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]
    public string Name {  get; set; }

    [Required(ErrorMessage = "Surname is required.")]
    [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]
    public string Surname { get; set; }
    public string? Faculty { get; set; }
    public ExamResults? ExamResults { get; set; }
    public DateTime RegistrationTime { get; set; }
    public ICollection<UserRole> Roles { get; set; }

}
