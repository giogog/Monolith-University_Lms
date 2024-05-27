using Contracts;
using Domain.Dtos;
using Domain.Models;

namespace Application.Services
{
    public class ApplicantService : IApplicantService
    {
        public ApplicantDto CreateApplicant(User user)
        {
            return new ApplicantDto(user.Name, user.Surname, user.UserName, user.Faculty,
                new ExamsCardDto(user.ExamResults.Grant, user.ExamResults.Results));
        }
    }
}
