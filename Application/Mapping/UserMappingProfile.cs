using System.Collections.Generic;
using Application.Dtos;
using Application.Dtos.UserDto;
using AutoMapper;
using Domain.Enities;
namespace Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Маппинг для получения пользователя по его идентификатору
            CreateMap<User, UserGetByIdResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            // Маппинг для получения всех пользователей
            CreateMap<User, BaseUserDto>();

            CreateMap<User, UserCreateResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.isAdmins, opt => opt.MapFrom(src => src.Admin));

            CreateMap<User, UserUpdateResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.isAdmins, opt => opt.MapFrom(src => src.Admin));
        }
    }
}