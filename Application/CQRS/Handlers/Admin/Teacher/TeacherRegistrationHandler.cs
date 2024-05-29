using Application.CQRS.Commands;
using Contracts;
using Domain.Dtos;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Handlers;

public class TeacherRegistrationHandler : IRequestHandler<TeacherRegistrationCommand, IdentityResult>
{
    private readonly IServiceManager _serviceManager;
    private readonly IRepositoryManager _repositoryManager;

    public TeacherRegistrationHandler(IServiceManager serviceManager,IRepositoryManager repositoryManager)
    {
        _serviceManager = serviceManager;
        _repositoryManager = repositoryManager;
    }
    public async Task<IdentityResult> Handle(TeacherRegistrationCommand request, CancellationToken cancellationToken)
    {
        var registration = new RegisterDto(request.PersonalID, request.Name, request.Surname,
            request.Email, request.Password, null
            , AcademicRole.Teacher, null);

        var registerResult = await _serviceManager.AuthorizationService.Register(registration);

        if (registerResult.Succeeded)
        {
            var user = await _repositoryManager.UserRepository.GetUser(u => u.UserName == request.PersonalID);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Code = "NotFound", Description = "User not found personalId for teacher creation" });
            Teacher teacher = new Teacher 
            { 
                Id = user.Id,
                Salary = request.Salary
            };
            await _repositoryManager.TeacherRepository.AddTeacher(teacher);
            var saveResult = await _repositoryManager.SaveAsync();
            if(saveResult == 0)
                return IdentityResult.Failed(new IdentityError { Code = "Couldn'tSaved", Description = "Teacher couldn't saved in Database" });
        }

        return registerResult;
    }
}
