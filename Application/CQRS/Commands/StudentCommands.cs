using Domain.Models;
using MediatR;

namespace Application.CQRS.Commands;

public record PayForSemesterCommand(decimal Ammount, string personalId):IRequest<Result<int>>;
public record EnrollToSubjectCommand(int subjectId,int lectureId,int seminarId, string personalId):IRequest<Result<int>>;