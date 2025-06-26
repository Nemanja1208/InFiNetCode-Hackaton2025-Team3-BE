using Application_Layer.UserAuth.Dtos;
using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using Application_Layer.Steps.Dtos;

namespace Application_Layer.Common.Mappings
{
    public class DataMappings : Profile
    {
        public DataMappings()
        {
            // Användare
            CreateMap<UserModel, UserDataDto>().ReverseMap();
            CreateMap<Step, CreateStepDto>().ReverseMap();

            // Idé-session med steg
            CreateMap<IdeaSession, IdeaSessionWithStepsDto>()
                .ForMember(dest => dest.IdeaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps.OrderBy(s => s.Order)));

            CreateMap<Step, StepDto>()
                .ForMember(dest => dest.StepId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));

        }
    }
}
