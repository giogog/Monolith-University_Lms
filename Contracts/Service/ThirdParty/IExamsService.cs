using Domain.Dtos;

namespace Contracts;

public interface IExamsService
{
    Task<ExamsCardDto> GetResultsFromThirdParty(string personalId);
}
