using AutoMapper;
using Contracts;
using Domain.CQRS.Queries;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class GetSubjectbyNameHandler : IRequestHandler<GetSubjectbyNameQuery, Result<SubjectDtoGet>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetSubjectbyNameHandler(IRepositoryManager repositoryManager,IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public async Task<Result<SubjectDtoGet>> Handle(GetSubjectbyNameQuery request, CancellationToken cancellationToken)
    {
        var subject = await _repositoryManager.SubjectRepository.GetByCondition(s=>s.Name.ToLower()==request.SubjectName.ToLower())
            .FirstOrDefaultAsync();
        if (subject == null)
            return Result<SubjectDtoGet>.Failed("NotFound", "Subject not found");

        var subjectdto = _mapper.Map<SubjectDtoGet>(subject);

        return Result<SubjectDtoGet>.Success(subjectdto);
    }
}
