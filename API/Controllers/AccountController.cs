using Application.CQRS.Commands;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

public class AccountController(IServiceManager serviceManager,IMediator mediator) : ApiController(serviceManager,mediator)
{

    [HttpPost("student-application")]
    public async Task<ActionResult> Register(StudentApplicationCommand studentApplicationCommand)
    {
        var registrationCheckUp = await mediator.Send(studentApplicationCommand);
        if (!registrationCheckUp.Succeeded)
            return BadRequest(registrationCheckUp.Errors);

        var sendmail =  await serviceManager.EmailService.SendConfirmationMail(Url, studentApplicationCommand.PersonalID);
        if (!sendmail.Succeeded)
            return BadRequest(sendmail.Errors);

        return Ok("Registration successful. Please check your email to confirm your account.");
    }

    [HttpPost("resend-confirmation/{PersonalId}")]
    public async Task<ActionResult> ResendConfirmation(string PersonalId)
    {
        var alreadyConfirmed = await serviceManager.AuthorizationService.CheckMailConfirmation(PersonalId);
        if(alreadyConfirmed.IsSuccess)
            return BadRequest(alreadyConfirmed.ErrorMessage);

        var emailSent = await serviceManager.EmailService.SendConfirmationMail(Url, PersonalId);
        if (!emailSent.Succeeded)
            return BadRequest(emailSent.Errors);

        return Ok("Please check your email to confirm your account.");
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        var loginCheckUp = await serviceManager.AuthorizationService.Login(loginDto);
        if (!loginCheckUp.Succeeded)
            if (loginCheckUp.Errors.Where(e => e.Code == "UserDoesNotExist").Any())
                return NotFound(loginCheckUp.Errors);
            else 
                return Unauthorized(loginCheckUp.Errors);

        return Ok(await serviceManager.AuthorizationService.Authenticate(user => user.UserName == loginDto.PersonalId));
    }

    [HttpGet("confirm-email")]
    public async Task<ActionResult> ConfirmEmail(int userId, string token)
    {
        var result = await serviceManager.EmailService.ConfirmEmailAsync(userId.ToString(), token);
        if (!result.Succeeded)
        {   
            return Redirect($"https://localhost:5003/confirmation?errormassage={"Email confirmation Failed"}");
        }
        await mediator.Send(new SendNewApplicantDataCommand(userId));
        return Redirect("https://localhost:5003/login");
    }
}
