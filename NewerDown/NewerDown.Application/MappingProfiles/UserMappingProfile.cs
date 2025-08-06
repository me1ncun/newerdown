using AutoMapper;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserDto, User>();
        
        CreateMap<User, UserDto>()
            .ForMember(x => x.FilePath, opt => opt.MapFrom(src => src.FileAttachment.FilePath));
    }
}