using Application.CQRS.Commands;
using Contracts;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CQRS.Handlers;

public class DeclineStudentApplicationHandler : IRequestHandler<DeclineStudentApplicationCommand, Result<string>>
{
    private readonly IRepositoryManager _repositoryManager;

    public DeclineStudentApplicationHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<string>> Handle(DeclineStudentApplicationCommand request, CancellationToken cancellationToken)
    {
        var user = await _repositoryManager.UserRepository.GetApplicantWithExamResultsAsync(u => u.UserName == request.personalId);
        if (user == null)
            return Result<string>.Failed("NotFound", "User was n't found with selected personaID");

        var result = await _repositoryManager.UserRepository.DeleteUser(user);
        if (!result.Succeeded)
            return Result<string>.Failed("DatabaseError","Unable to delete user");

        return Result<string>.Success("User removed successfully");
    }


}
