using Application.Dtos.UserDto;
using AutoMapper;
using Domain.Enities;
namespace Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserGetByIdResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

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