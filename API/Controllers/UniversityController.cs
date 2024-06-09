using Domain;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Dtos;
using MediatR;
using Domain.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;
using Application.CQRS.Commands;

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


    [Authorize(Roles = "Admin")]
    [HttpGet("applicants")]
    public async Task<ActionResult<IEnumerable<ApplicantDto>>> GetApplicants()
    {
        var result = await mediator.Send(new GetApplicantsQuery());
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return Ok(result.Data);
    }


    //[Authorize(Roles = "Admin")]
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


    [Authorize(Roles = "Admin")]
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


    //[Authorize(Roles = "Admin")]
    [HttpPost("subject-register")]
    public async Task<IActionResult> SubjectRegistration([FromBody]SubjectRegistrationCommand subjectRegistrationCommand)
    {
        var result = await mediator.Send(subjectRegistrationCommand);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return Ok("Subject registered successfully");
    }


    [HttpPut("subject-update")]
    public async Task<IActionResult> SubjectUpdate([FromBody] SubjectUpdateCommand subjectRegistrationCommand)
    {
        var result = await mediator.Send(subjectRegistrationCommand);
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Subject Updated successfully");
    }
    
    
    [HttpPost("lecture-add")]
    public async Task<IActionResult> LectureAdd([FromBody] LectureAddCommand lectureAddCommand)
    {
        var result = await mediator.Send(lectureAddCommand);
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Lecture add successfully");
    }
    
    
    [HttpPost("seminar-add")]
    public async Task<IActionResult> SeminarAdd([FromBody] SeminarAddCommand seminarAddCommand)
    {
        var result = await mediator.Send(seminarAddCommand);
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Seminar add successfully");
    }

    [HttpPut("lecture-activation/{lectureId}")]
    public async Task<IActionResult> ActivateLecture(int lectureId)
    {
        var result = await mediator.Send(new LectureActivationCommand(lectureId,true));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Lecture deleted successfully");
    }
    [HttpPut("lecture-deactivation/{lectureId}")]
    public async Task<IActionResult> DeactivateLecture(int lectureId)
    {
        var result = await mediator.Send(new LectureActivationCommand(lectureId,false));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Lecture deleted successfully");
    }


    [HttpPut("seminar-activation/{seminarId}")]
    public async Task<IActionResult> ActivateSeminar(int seminarId)
    {
        var result = await mediator.Send(new SeminarActivationCommand(seminarId,true));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Seminar Deleted successfully");
    }
    [HttpPut("seminar-deactivation/{seminarId}")]
    public async Task<IActionResult> DeactivateSeminar(int seminarId)
    {
        var result = await mediator.Send(new SeminarActivationCommand(seminarId,false));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Seminar Deleted successfully");
    }


    [HttpPut("subject-activation/{subjectId}")]
    public async Task<IActionResult> ActivateSubject(int subjectId)
    {
        var result = await mediator.Send(new SubjectActivationCommand(subjectId,true));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Subject Activated successfully");
    }


    [HttpPut("subject-deactivation/{subjectId}")]
    public async Task<IActionResult> DeactivateSubject(int subjectId)
    {
        var result = await mediator.Send(new SubjectActivationCommand(subjectId,false));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Subject Activated successfully");
    }

    [HttpGet("subject/{subjectName}")]
    public async Task<ActionResult<SubjectDtoGet>> SearchSubject(string subjectName)
    {
        var result = await mediator.Send(new GetSubjectbyNameQuery(subjectName));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }

}
