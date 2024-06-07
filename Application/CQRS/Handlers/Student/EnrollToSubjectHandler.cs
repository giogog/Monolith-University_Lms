using Application.CQRS.Commands;
using Application.SignalR;
using Contracts;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Handlers;

public class EnrollToSubjectHandler : IRequestHandler<EnrollToSubjectCommand, Result<int>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IHubContext<EnrollmentHub> _hubContext;

    public EnrollToSubjectHandler(IRepositoryManager repositoryManager, IHubContext<EnrollmentHub> hubContext)
    {
        _repositoryManager = repositoryManager;
        _hubContext = hubContext;
    }
    public async Task<Result<int>> Handle(EnrollToSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repositoryManager.SubjectRepository.GetByCondition(s => s.Id == request.subjectId)
            .Include(s=>s.Seminars)
                .ThenInclude(s=>s.StudentEnrollments)
            .Include(s=>s.Lectures)
                .ThenInclude(l=>l.StudentEnrollments)
            .FirstOrDefaultAsync();
        if(subject == null)
            return Result<int>.Failed("NotFound", "Subject on this Id not found");

        var lecture = subject.Lectures.Where(l=>l.Id==request.lectureId).FirstOrDefault();
        var seminar = subject.Seminars.Where(s => s.Id == request.seminarId).FirstOrDefault();


        if (lecture == null)
            return Result<int>.Failed("NotFound", "Lecture on this Id not found");
        if (lecture.LectureCapacity == lecture.TakenPlaces)
            return Result<int>.Failed("Unavailable","No more free place for this lecture");



        int? newSeminarId = null;

        if(seminar != null)
            newSeminarId = request.seminarId;

        if (seminar.SeminarCapacity <= seminar.TakenPlaces)
            return Result<int>.Failed("Unavailable", "No more free places for this seminar");


        var student = await _repositoryManager.StudentRepository.GetByConditionAsync(s => s.User.UserName == request.personalId).FirstOrDefaultAsync();
        if (student == null)
            return Result<int>.Failed("NotFound", "Student on this personalId not found");

        try
        {
            await _repositoryManager.BeginTransactionAsync();
            var studentEnrollment = new StudentEnrollment(subject.gradeTypes.Count()) 
            {
                LectureId = request.lectureId,
                SeminarId = newSeminarId,
                StudentId = student.Id,
                IsActive = true
            };
            _repositoryManager.EnrollmentRepository.AddEnrollment(studentEnrollment);
            await _repositoryManager.SaveAsync();
            await _repositoryManager.CommitTransactionAsync();
            await _hubContext.Clients.Group("Students").SendAsync("NewLectureCapacity", lecture.StudentEnrollments.Count()+1);
            if(newSeminarId != null)
                await _hubContext.Clients.Group("Students").SendAsync("NewSeminarCapacity", seminar.StudentEnrollments.Count() + 1);
            return Result<int>.Success(1);
        }
        catch (Exception ex)
        {
            await _repositoryManager.RollbackTransactionAsync();
            return Result<int>.Failed("Failed",ex.Message);
        }
    }
}
