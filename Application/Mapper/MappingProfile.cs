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
    }

}
public class DayOfWeekResolver : IValueResolver<ScheduleProperty, ScheduleTimesDto, string>
{
    public string Resolve(ScheduleProperty source, ScheduleTimesDto destination, string destMember, ResolutionContext context)
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(source.DayOfWeek);
    }
}
