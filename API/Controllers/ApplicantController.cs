using Application.CQRS.Commands;
using Contracts;
using Domain.CQRS.Queries;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ApplicantController(IServiceManager serviceManager, IMediator mediator) : ApiController(serviceManager, mediator)
{
    [Authorize(Roles = "Admin")]
    [HttpGet("applicants")]
    public async Task<ActionResult<IEnumerable<ApplicantDto>>> GetApplicants()
    {
        var result = await mediator.Send(new GetApplicantsQuery());
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpPost("approve-application/{personalId}")]
    public async Task<IActionResult> ApproveApplication(string personalId)
    {
        var result = await mediator.Send(new ApproveStudentApplicationCommand(personalId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else 
                return BadRequest(result.Message);

        return Ok(result.Data);

    }
    [HttpDelete("decline-application/{personalId}")]
    public async Task<IActionResult> DeclineApplication(string personalId)
    {
        var result = await mediator.Send(new DeclineStudentApplicationCommand(personalId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
