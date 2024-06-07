using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class SubjectService: ISubjectService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public SubjectService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public async Task<Result<int>> RegisterNewSubject(SubjectDto subjectDto)
    {
        var subject = _mapper.Map<Subject>(subjectDto);
        subject.Faculty = await _repositoryManager.FacultyRepository.GetFacultyByNameAsync(subjectDto.FacultyName);

        try
        {
            await _repositoryManager.SubjectRepository.AddSubject(subject);
            var saveResult = await _repositoryManager.SaveAsync();
            return Result<int>.Success(saveResult);
        }
        catch (Exception ex)
        {
            return Result<int>.Failed("SaveError",ex.Message);
        }
    }
    public async Task<IEnumerable<Subject>> GetActiveSubjectsByStudentId(int studentId)
    {
        return await _repositoryManager.EnrollmentRepository
            .GetByCondition(en => en.IsActive && en.StudentId == studentId)
            .Select(s => s.Lecture.Subject)
            .ToArrayAsync();

    }

    public async Task<IEnumerable<Subject>> GetPassedSubjectsByStudentId(int studentId)
    {
        var studentEnrollments = await _repositoryManager.EnrollmentRepository
            .GetByCondition(en => en.StudentId == studentId)
            .Include(en => en.Lecture).ThenInclude(l => l.Subject).ToArrayAsync();

        return studentEnrollments.Select(en => en.Lecture.Subject);
    }

    public async Task<IEnumerable<Subject>> GetSubjectsByStudentId(int studentId)
    {
        return await _repositoryManager.EnrollmentRepository
            .GetByCondition(en => en.StudentId == studentId)
            .Select(s => s.Lecture.Subject)
            .ToArrayAsync();

    }
}
