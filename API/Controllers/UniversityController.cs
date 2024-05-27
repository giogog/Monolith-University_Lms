using Domain;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Dtos;
using MediatR;
using Domain.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class UniversityController(IServiceManager serviceManager,IMediator mediator):ApiController(serviceManager,mediator)
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
