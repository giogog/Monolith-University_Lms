using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Contracts;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers; 

public class StudentController(IServiceManager serviceManager, IMediator mediator) : ApiController(serviceManager, mediator)
{
    //[Authorize(Roles = "Student,Admin")]
    [HttpGet("data/{personalId}")]
    public async Task<ActionResult<StudentCardDto>> GetStudentData(string personalId)
    {
        var result = await mediator.Send(new GetStudentDataQuery(personalId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpGet("card/{personalId}")]
    public async Task<ActionResult<StudentCardDto>> GetStudentCard(string personalId)
    {
        var result = await mediator.Send(new GetStudentCardQuery(personalId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }
    [Authorize(Roles = "Student")]
    [HttpGet("selected-subjects/{personalId}")]
    public async Task<ActionResult<IEnumerable<StudentActiveEnrollmentsDto>>> GetEnrolledSubjects(string personalId)
    {
        var result = await mediator.Send(new GetActiveEnrolledSubjectsQuery(personalId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpGet("schedule/{personalId}")]
    public async Task<ActionResult<StudentScheduleDto>> GetStudentSchedule(string personalId)
    {
        var result = await mediator.Send(new GetStudentScheduleQuery(personalId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }
    [Authorize(Roles = "Student")]
    [HttpGet("subject-by-faculty/{faculty}")]
    public async Task<ActionResult<IEnumerable<SubjectByFacultyDto>>> GetSubjectsByFacultyName(string faculty)
    {
        var result = await mediator.Send(new GetSubjectsByFacultyQuery(faculty));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }
    [Authorize(Roles = "Student")]
    [HttpGet("subject-by-id/{subjectId}")]
    public async Task<ActionResult<EnrollmentSubjectDto>> GetSubjectsById(int subjectId)
    {
        var result = await mediator.Send(new GetSubjectQuery(subjectId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }


    [HttpPut("payment")]
    public async Task<ActionResult> PayForSemester([FromBody] PayForSemesterCommand payForSemesterCommand)
    {
        var result = await mediator.Send(payForSemesterCommand);
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Payment Success");
    }
    [Authorize(Roles = "Student")]
    [HttpPost("enrollment-to-subject")]
    public async Task<ActionResult> EnrollToSubject([FromBody]EnrollToSubjectCommand enrollToSubjectCommand)
    {
        var result = await mediator.Send(enrollToSubjectCommand);
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok("Enrolled Successfully");
    }
}
