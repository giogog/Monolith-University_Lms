using Domain.Dtos;
using Domain.Models;
using MediatR;

namespace Domain.CQRS.Queries;

public record GetApplicantsQuery():IRequest<Result<IEnumerable<ApplicantDto>>>;
