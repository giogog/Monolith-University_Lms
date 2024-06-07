using Application.CQRS.Queries;
using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class GetSubjectsByFacultyHandler : IRequestHandler<GetSubjectsByFacultyQuery, Result<IEnumerable<SubjectByFacultyDto>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetSubjectsByFacultyHandler(IRepositoryManager repositoryManager,IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<SubjectByFacultyDto>>> Handle(GetSubjectsByFacultyQuery request, CancellationToken cancellationToken)
    {
        var faculty = await _repositoryManager.FacultyRepository.GetByCondition(f=>f.Name == request.Faculty).Include(f=>f.Subjects).FirstOrDefaultAsync();
        if (faculty == null)
            return Result<IEnumerable<SubjectByFacultyDto>>.Failed("NotFound","Faculty with that name not found");
        if (!faculty.Subjects.Any())
            return Result<IEnumerable<SubjectByFacultyDto>>.Failed("NotFound", "Subjects on this Faculty not found");

        var subjectDtoList = _mapper.Map<List<SubjectByFacultyDto>>(faculty.Subjects.ToList());

        return Result<IEnumerable<SubjectByFacultyDto>>.Success(subjectDtoList);
    }
}
