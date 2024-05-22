using Application.CQRS.Commands;
using Contracts;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(IServiceManager _serviceManager,IMediator mediator) : ApiController(_serviceManager,mediator)
{

    [HttpPost("student-application")]
    public async Task<ActionResult> Register(StudentApplicationCommand studentApplicationCommand)
    {
        var registrationCheckUp = await mediator.Send(studentApplicationCommand);

        if (!registrationCheckUp.Succeeded)
            return BadRequest(registrationCheckUp.Errors);
        var sendmail =  await _serviceManager.EmailService.SendConfirmationMail(Url, studentApplicationCommand.PersonalID);

        if (!sendmail.Succeeded)
            return BadRequest(sendmail.Errors);

        return Ok("Registration successful. Please check your email to confirm your account.");
    }

    [HttpPost("resend-confirmation")]
    public async Task<ActionResult> ResendConfirmation(string PersonalId)
    {
        var alreadyConfirmed = await _serviceManager.AuthorizationService.CheckMailConfirmation(PersonalId);

        if(alreadyConfirmed.IsSuccess)
            return BadRequest(alreadyConfirmed.ErrorMessage);


        var emailSent = await _serviceManager.EmailService.SendConfirmationMail(Url, PersonalId);

        if (!emailSent.Succeeded)
            return BadRequest(emailSent.Errors);

        return Ok("Please check your email to confirm your account.");
    }


    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        var loginCheckUp = await _serviceManager.AuthorizationService.Login(loginDto);

        if (!loginCheckUp.Succeeded)
            return BadRequest(loginCheckUp.Errors);


        return Ok(await _serviceManager.AuthorizationService.Authenticate(user => user.UserName == loginDto.Username));
    }

    [HttpGet("confirm-email")]
    public async Task<ActionResult<LoginResponseDto>> ConfirmEmail(string personalId, string token)
    {
        var result = await _serviceManager.EmailService.ConfirmEmailAsync(personalId, token);

        if (!result.Succeeded)
        {
            return BadRequest("Error confirming email.");
        }

        return Ok(await _serviceManager.AuthorizationService.Authenticate(user => user.UserName == personalId));

    }


}
