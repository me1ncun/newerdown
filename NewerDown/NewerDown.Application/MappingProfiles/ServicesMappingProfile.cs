using AutoMapper;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class ServicesMappingProfile : Profile
{
    public ServicesMappingProfile()
    {
        CreateMap<AddServiceDto, Service>();
        
        CreateMap<Service, ServiceDto>()
            .ReverseMap();

        CreateMap<UpdateServiceDto, Service>();
    }
}