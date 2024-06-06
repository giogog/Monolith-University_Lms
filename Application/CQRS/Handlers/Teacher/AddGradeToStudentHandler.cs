using Application.CQRS.Commands;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class AddGradeToStudentHandler : IRequestHandler<AddGradeToStudentCommand, Result<double[]>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager _serviceManager;

    public AddGradeToStudentHandler(IRepositoryManager repositoryManager,IServiceManager serviceManager)
    {
        _repositoryManager = repositoryManager;
        _serviceManager = serviceManager;
    }
    public async Task<Result<double[]>> Handle(AddGradeToStudentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _repositoryManager.EnrollmentRepository.GetEnrollmentByIds(en => en.Id == request.enrollmentId);
        if (enrollment == null)
            return Result<double[]>.Failed("NotFound", "Enrollment not found");

        var gradeSystemResult = await _serviceManager.GradeService.GetGradeSystem(enrollment.Grades,request.subjectId);
        if (!gradeSystemResult.IsSuccess)
            return Result<double[]>.Failed(gradeSystemResult.Code, gradeSystemResult.Message);

        var addGradeResult = _serviceManager.GradeService.AddGradeToStudent(new GradeDto(gradeSystemResult.Data,request.GradeType,request.Grade));
        if (!addGradeResult.IsSuccess)
            return Result<double[]>.Failed(addGradeResult.Code, addGradeResult.Message);

        enrollment.Grades = addGradeResult.Data;

        await _repositoryManager.EnrollmentRepository.UpdateEnrollment(enrollment);

        try
        {
            var saveResult = await _repositoryManager.SaveAsync();
            return Result<double[]>.Success(enrollment.Grades);
        }
        catch (Exception ex)
        {
            return Result<double[]>.Failed("SavingError", ex.Message);
        }
    }
}
