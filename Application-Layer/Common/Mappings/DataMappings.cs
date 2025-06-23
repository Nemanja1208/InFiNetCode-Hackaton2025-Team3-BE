using Application_Layer.UserAuth.Dtos;
using Domain_Layer.Models;
using AutoMapper;

namespace Application_Layer.Common.Mappings
{
    public class DataMappings : Profile
    {
        public DataMappings() 
        {
            CreateMap<UserModel, UserDataDto>().ReverseMap();
        }
        
    }
}
