using Application.Dtos.UserDto.Request;
using Application.Dtos.UserDto.Responce;
using AutoMapper;
using Domain.Enities;

namespace Application.Mapping;

/// <summary>
///     Маппинг для пользователя
/// </summary>
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

        CreateMap<UserCreateResponse, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src.isAdmins));

        CreateMap<UserDeleteRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<UserUpdateRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src.isAdmins));
        CreateMap<UserCreateRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src.Admin));
    }
}