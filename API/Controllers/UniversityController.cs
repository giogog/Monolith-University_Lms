using Domain;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class UniversityController(IServiceManager serviceManager):ApiController(serviceManager)
{ 
    [HttpGet("faculty")]
    public async Task<ActionResult<IEnumerable<string>>> GetFaculties()
    {
        var result = await serviceManager.UniversityService.GetFacultyNames();
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return Ok(result.Data);
    }
}
