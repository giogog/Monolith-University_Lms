using Domain.Dtos;

namespace Contracts;

public interface IThirdpartyService
{

    Task<ExamsCardDto> GenerateRandomResultData();
}
