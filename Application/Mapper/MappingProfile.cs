using AutoMapper;
using Domain.Dtos;
using Domain.Models;
using Domain.Props;
using System.Globalization;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<Subject, SubjectDtoGet>().ReverseMap();
        CreateMap<ScheduleProperty, ScheduleTimesDto>()
            .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom<DayOfWeekResolver>())
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime)).ReverseMap();


        CreateMap<Student, StudentDataDto>()
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.User.Name))
            .ForCtorParam("Surname", opt => opt.MapFrom(src => src.User.Surname))
            .ForCtorParam("Email", opt => opt.MapFrom(src => src.User.Email))
            .ForCtorParam("SemesterPay", opt => opt.MapFrom(src => src.SemesterPay))
            .ForCtorParam("Status", opt => opt.MapFrom(src => src.Status)).ReverseMap();
        CreateMap<Subject, SubjectByFacultyDto>()
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("subjectId", opt => opt.MapFrom(src => src.Id));


    }

}
public class DayOfWeekResolver : IValueResolver<ScheduleProperty, ScheduleTimesDto, string>
{
    public string Resolve(ScheduleProperty source, ScheduleTimesDto destination, string destMember, ResolutionContext context)
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(source.DayOfWeek);
    }
}
