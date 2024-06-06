using Application.CQRS.Queries;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class GetTeacherDataHandler : IRequestHandler<GetTeacherDataQuery, Result<TeacherDto>>
{
    private readonly IRepositoryManager _repositoryManager;

    public GetTeacherDataHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public async Task<Result<TeacherDto>> Handle(GetTeacherDataQuery request, CancellationToken cancellationToken)
    {
        var teacher = await _repositoryManager.TeacherRepository.GetByCondition(t => t.User.UserName == request.personalId)
            .Include(t=>t.User)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (teacher == null)
            return Result<TeacherDto>.Failed("NotFound","Teacher not Found");

        return Result<TeacherDto>.Success(new TeacherDto(teacher.User.UserName,teacher.User.Name,teacher.User.Surname,teacher.User.Email,teacher.Salary));
    }
}
