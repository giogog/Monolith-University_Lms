using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class User:IdentityUser<int>
{
    [RegularExpression(@"^5\d{8}$", ErrorMessage = "Phone number must start with 5 and must be 9 digits long.")]
    public string? PhoneNumber { get; set; }
    public ICollection<UserRole> Roles { get; set; }

}
