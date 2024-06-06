using Domain.Dtos;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Commands;

public record AddGradeToStudentCommand(int subjectId,int enrollmentId, string GradeType, double Grade) : IRequest<Result<double[]>>;

