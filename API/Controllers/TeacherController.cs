using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Contracts;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class TeacherController(IServiceManager serviceManager, IMediator mediator) : ApiController(serviceManager, mediator)
{
    [HttpGet("teacher/{personalId}")]
    public async Task<ActionResult<TeacherDto>> GetTeacherData(string personalId)
    {
        var result = await mediator.Send(new GetTeacherDataQuery(personalId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }

    //[HttpGet("teacher-schedule/{personalId}")]
    //public async Task<ActionResult<IEnumerable<TeacherScheduleDto>>> GetTeacherSchedule(string personalId)
    //{
    //    var result = await mediator.Send(new GetTeacherScheduleQuery(personalId));
    //    if (!result.IsSuccess)
    //        if (result.Code == "NotFound")
    //            return NotFound(result.Message);
    //        else
    //            return BadRequest(result.Message);
    //    return Ok(result.Data);
    //}

    [HttpGet("teacher-subjects/{personalId}")]
    public async Task<ActionResult<IEnumerable<TeacherScheduleDto>>> GetTeacherSubjects(string personalId)
    {
        var result = await mediator.Send(new GetTeacherSubjectsQuery(personalId));
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpGet("teacher-class-enrollments/{classType}/{classId}")]
    public async Task<ActionResult<IEnumerable<StudentEnrollmentDto>>> GetTeacherClassEnrollments(string classType,int classId)
    {
        
        if(classType.ToLower() == "lecture")
        {
            var LectureResult = await mediator.Send(new GetLectureEnrollmentsQuery(classId));
            if (!LectureResult.IsSuccess)
                if (LectureResult.Code == "NotFound")
                    return NotFound(LectureResult.Message);
                else
                    return BadRequest(LectureResult.Message);
            return Ok(LectureResult.Data);
        }if (classType.ToLower() == "seminar") 
        {
            var SeminarResult = await mediator.Send(new GetSeminarEnrollmentsQuery(classId));
            if (!SeminarResult.IsSuccess)
                if (SeminarResult.Code == "NotFound")
                    return NotFound(SeminarResult.Message);
                else
                    return BadRequest(SeminarResult.Message);
            return Ok(SeminarResult.Data);
        }
        return BadRequest("Wrong class type");

    }

    [HttpPut("teacher-grade-to-student")]
    public async Task<ActionResult<IEnumerable<TeacherScheduleDto>>> AddGradeToStudent([FromBody] AddGradeToStudentCommand addGradeToStudentCommand)
    {
        var result = await mediator.Send(addGradeToStudentCommand);
        if (!result.IsSuccess)
            if (result.Code == "NotFound")
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        return Ok(result.Data);
    }
}
