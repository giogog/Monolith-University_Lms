﻿using Application.CQRS.Commands;
using AutoMapper;
using Contracts;
using Domain.Dtos;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Handlers;

public class LectureAddHandler : IRequestHandler<LectureAddCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IServiceManager _serviceManager;
    private readonly IMapper _mapper;

    public LectureAddHandler(IRepositoryManager repositoryManager, IServiceManager serviceManager,IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _serviceManager = serviceManager;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(LectureAddCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repositoryManager.SubjectRepository.GetSubjectById(request.subjectId);
        if (subject == null)
            return Result<int>.Failed("NotFound", "Subject not found");

        var lectureDto = new LectureDto(request.LectureCapacity, request.TeacherPersonalId
            , request.DayOfWeek, request.StartTime, request.EndTime);

        var lectureAddResult = await _serviceManager.LectureService.RegisterLecture(subject.Id, lectureDto);

        if (!lectureAddResult.IsSuccess)
            return Result<int>.Failed(lectureAddResult.Code, lectureAddResult.Message);

        try
        {
            var saveResult = await _repositoryManager.SaveAsync();
            return Result<int>.Success(saveResult);
        }
        catch (Exception ex)
        {
            return Result<int>.Failed("SavingError", ex.Message);
        }


    }
}
