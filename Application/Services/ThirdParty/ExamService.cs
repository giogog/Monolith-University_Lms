using Contracts;
using Domain.Dtos;
using System.Net.Http;
using System.Text.Json;

namespace Application.Services;

public class ExamService:IExamsService
{
    private readonly HttpClient _httpClient;

    public ExamService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ExamsCardDto> GetResultsFromThirdParty(string personalId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:5001/api/Thirdparty/{personalId}");
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        var data = JsonSerializer.Deserialize<ExamsCardDto>(jsonString, options);

        return data;
    }


}
