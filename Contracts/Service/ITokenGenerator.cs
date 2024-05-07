using Domain.Models;

namespace Contracts;

public interface ITokenGenerator
{
    Task<string> GenerateToken(User user);
    Task<string> GenerateMailTokenCode(User user);

}
