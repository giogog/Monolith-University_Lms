using Domain.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.Commands;

public record StudentApplicationCommand(string PersonalID, string Name, string Surname, string Email, string Password, string Faculty) :IRequest<IdentityResult>;

public record SendNewApplicantDataCommand(int userId) : IRequest<Result<ApplicantDto>>;
