using Application.CQRS.Queries;
using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class GetStudentDataHandler : IRequestHandler<GetStudentDataQuery, Result<StudentDataDto>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetStudentDataHandler(IRepositoryManager repositoryManager,IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        this._mapper = mapper;
    }
    public async Task<Result<StudentDataDto>> Handle(GetStudentDataQuery request, CancellationToken cancellationToken)
    {
        var student = await _repositoryManager.StudentRepository.GetByConditionAsync(st=>st.User.UserName == request.personalId)
            .Include(st=>st.User)
            .FirstOrDefaultAsync();

        if (student == null)
            return Result<StudentDataDto>.Failed("NotFound","Student not found");

        var dto = _mapper.Map<StudentDataDto>(student);
        return Result<StudentDataDto>.Success(dto);

    }
}
