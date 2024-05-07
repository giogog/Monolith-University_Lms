using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Role:IdentityRole<int>
{
    public ICollection<UserRole> Users { get; set; }
}
