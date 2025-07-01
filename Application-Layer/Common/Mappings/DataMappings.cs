﻿using Application_Layer.UserAuth.Dtos;
using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using Application_Layer.Steps.Dtos;
using Application_Layer.IdeaSessions.Dto;
using Application_Layer.IdeaSessions.Commands;

namespace Application_Layer.Common.Mappings
{
    public class DataMappings : Profile
    {
        public DataMappings()
        {
            // Användare
            CreateMap<UserModel, UserDataDto>().ReverseMap();
            CreateMap<Step, CreateStepRequestDto>().ReverseMap();
            CreateMap<Step, CreateStepResponseDto>().ReverseMap();

            // Idé-session med steg
            CreateMap<IdeaSession, IdeaSessionWithStepsDto>()
                .ForMember(dest => dest.IdeaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps.OrderBy(s => s.Order)));

            CreateMap<Step, StepDto>()
                .ForMember(dest => dest.StepId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));

            // Mappa från DTO till Command
            CreateMap<CreateIdeaSessionDto, CreateIdeaSessionCommand>();

            // Mappa från Command till entity
            CreateMap<CreateIdeaSessionCommand, IdeaSession>();

            // Mappa från entity till Output-DTO
            CreateMap<IdeaSession, IdeaSessionDto>()
            .ForMember(dest => dest.IdeaId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
