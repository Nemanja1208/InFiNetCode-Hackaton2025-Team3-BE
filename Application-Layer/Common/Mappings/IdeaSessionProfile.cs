using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Application_Layer.IdeaSessions.Dto;
using Application_Layer.IdeaSessions.Commands;
using Domain_Layer.Models;

namespace Application_Layer.Common.Mappings
{
    public class IdeaSessionProfile : Profile
    {
        public IdeaSessionProfile()
        {
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