using Contracts;
using Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ThirdpartController(IServiceManager serviceManager) : ApiController(serviceManager)
    {
        [HttpGet("{personalId}")]
        public async Task<ActionResult<ExamsCardDto>> GetResults(string personalId)
        {
            var results = await serviceManager.ThirdpartyService.GenerateRandomResultData();

            return Ok(results);
        }

    }
}
