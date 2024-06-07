using Application.CQRS.Commands;
using Contracts;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class PayForSemesterHandler : IRequestHandler<PayForSemesterCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;

    public PayForSemesterHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<int>> Handle(PayForSemesterCommand request, CancellationToken cancellationToken)
    {
        var student = await _repositoryManager.StudentRepository.GetByConditionAsync(s => s.User.UserName == request.personalId).FirstOrDefaultAsync();
        if (student == null)
            return Result<int>.Failed("NotFound","Student not found");

        student.Balance += request.Ammount;
        if(student.Balance >= student.SemesterPay)
            student.Status = AcademicStatus.Active;

        await _repositoryManager.StudentRepository.UpdateStudentAsync(student);


        try
        {
            var saveResult = await _repositoryManager.SaveAsync();
            return Result<int>.SuccesfullyUpdated(saveResult, saveResult);
        }
        catch (Exception ex)
        {
            return Result<int>.Failed("SavingError", ex.Message);
        }
    }
}
