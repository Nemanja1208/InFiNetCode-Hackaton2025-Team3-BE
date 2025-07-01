// Application-Layer/Common/Mapping/IdeaSessionProfile.cs
using AutoMapper;
using Application_Layer.IdeaSessions.DTOs;
using Domain_Layer.Models;

namespace Application.Common.Mapping
{
    public class IdeaSessionProfile : Profile
    {
        public IdeaSessionProfile()
        {
            CreateMap<IdeaSession, IdeaSessionWithStepsDto>();
        }
    }
}
