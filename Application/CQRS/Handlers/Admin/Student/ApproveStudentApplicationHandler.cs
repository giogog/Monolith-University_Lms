using Application.CQRS.Commands;
using Contracts;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class ApproveStudentApplicationHandler : IRequestHandler<ApproveStudentApplicationCommand, Result<string>>
{
    private readonly IRepositoryManager _repositoryManager;

    public ApproveStudentApplicationHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<string>> Handle(ApproveStudentApplicationCommand request, CancellationToken cancellationToken)
    {
        //get necessery data
        var user = await _repositoryManager.UserRepository.GetApplicantWithExamResultsAsync(u => u.UserName == request.personalId);
        if (user == null)
            return Result<string>.Failed("NotFound","User was n't found with selected personaID");

        var university = await _repositoryManager.UniversityRepository.GetUniversityAsync();
        var currentRole = await _repositoryManager.RoleRepository.GetRoleByNameAsync("Applicant");

        //build up new student
        decimal semesterPaymentDecimal = (decimal)university.SemesterPayment;
        decimal grantDecimal = (decimal)user.ExamResults.Grant;
        Student student = new Student
        {
            Id = user.Id,
            Faculty = await _repositoryManager.FacultyRepository.GetFacultyByNameAsync(user.Faculty),
            Grant = user.ExamResults.Grant,
            SemesterPay = semesterPaymentDecimal * grantDecimal,
            YearlyAvailableCredits = 65
        };
        
        //add student
        await _repositoryManager.StudentRepository.AddStudentAsync(student);

        //remove applicant status from user
        await _repositoryManager.UserRoleRepository.DeleteRoleToUserAsync(user.Id, currentRole.Id);

        //assign user to student role 
        var assignStudentRole = await _repositoryManager.UserRepository.AddToRole(user, "Student");
        if (!assignStudentRole.Succeeded)
            return Result<string>.Failed("RoleAssign","Can't assign user to Student Role");

        return Result<string>.Success($"User approved Sucessfully: {request.personalId}");
                     
    }
}
