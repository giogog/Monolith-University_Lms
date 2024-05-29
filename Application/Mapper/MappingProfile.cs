using AutoMapper;
using Domain.Dtos;
using Domain.Models;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<Subject, SubjectDtoGet>().ReverseMap();
    }

}

