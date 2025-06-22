using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application_Layer.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<IdeaSession, IdeaSessionWithStepsDto>()
            .ForMember(dest => dest.IdeaId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Steps, static opt => opt.MapFrom(src => src.Steps.OrderBy(s => s.StepOrder)));

        CreateMap<Step, StepDto>()
            .ForMember(dest => dest.StepId, opt => opt.MapFrom(src => src.Id));
    }
}
