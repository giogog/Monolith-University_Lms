﻿using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;

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
            return Result<int>.SuccesfullySaved(saveResult, saveResult);
        }
        catch (Exception ex)
        {
            return Result<int>.Failed("SaveError",ex.Message);
        }
    }
}
