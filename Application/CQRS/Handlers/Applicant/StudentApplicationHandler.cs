using Application.CQRS.Commands;
using Contracts;
using Domain.Dtos;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using Domain.Models;

namespace Application.CQRS.Handlers;

public class StudentApplicationHandler : IRequestHandler<StudentApplicationCommand, IdentityResult>
{
    private readonly IServiceManager _serviceManager;

    public StudentApplicationHandler(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }
    public async Task<IdentityResult> Handle(StudentApplicationCommand request, CancellationToken cancellationToken)
    {
        var examCard = await _serviceManager.ExamsService.GetResultsFromThirdParty(request.PersonalID);
        if (examCard == null)
            return IdentityResult.Failed(new IdentityError { Code = "ExamResultsNotFount",Description = "Exam Results on this PersonalId was not found" });

        var examResults = new ExamResults(examCard.Results.Count()) 
        { 
            Grant = examCard.Grant,
            SubjectNames = examCard.Results.Select(e=>e.Subject).ToArray(),
            SubjectGrades = examCard.Results.Select(e => e.Grade).ToArray()

        };
        var registration = new RegisterDto(request.PersonalID, request.Name,request.Surname, 
            request.Email,request.Password,request.Faculty
            ,AcademicRole.Applicant,examResults);

        var registerResult = await _serviceManager.AuthorizationService.Register(registration);
        return registerResult;
 
    }
}
