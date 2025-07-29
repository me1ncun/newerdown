using AutoMapper;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserDto, User>();
    }
}