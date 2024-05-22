using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class EmailResult:IdentityResult
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}
