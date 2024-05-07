using Contracts;
using Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IServiceManager _serviceManager;


    public AccountController(IServiceManager serviceManager) => _serviceManager = serviceManager;

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var registrationCheckUp = await _serviceManager.AuthorizationService.Register(registerDto);

        if (!registrationCheckUp.Succeeded)
            return BadRequest(registrationCheckUp.Errors);

        var emailSent = await _serviceManager.EmailService.SendConfirmationMail(Url, registerDto.Username);

        if (!emailSent.IsSuccess)
            return BadRequest(emailSent.ErrorMessage);

        return Ok("Registration successful. Please check your email to confirm your account.");
    }

    [HttpPost("resend-confirmation")]
    public async Task<ActionResult> ResendConfirmation(string Username)
    {
        var alreadyConfirmed = await _serviceManager.AuthorizationService.CheckMailConfirmation(Username);

        if(alreadyConfirmed.IsSuccess)
            return BadRequest(alreadyConfirmed.ErrorMessage);


        var emailSent = await _serviceManager.EmailService.SendConfirmationMail(Url, Username);

        if (!emailSent.IsSuccess)
            return BadRequest(emailSent.ErrorMessage);

        return Ok("Registration successful. Please check your email to confirm your account.");
    }

    [HttpPost("assign-role")]
    public async Task<ActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
    {

        var result = await _serviceManager.AuthorizationService.AssignRole(null, assignRoleDto.Roles, assignRoleDto.Username);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        return Ok($"Role Robot Assigned Successfully to User: {assignRoleDto.Username}");

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
    public async Task<ActionResult<LoginResponseDto>> ConfirmEmail(int userId, string token)
    {
        var result = await _serviceManager.EmailService.ConfirmEmailAsync(userId.ToString(), token);

        if (!result.Succeeded)
        {
            return BadRequest("Error confirming email.");
        }

        return Ok(await _serviceManager.AuthorizationService.Authenticate(user => user.Id == userId));

    }


}
