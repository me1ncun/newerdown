using System.Net.NetworkInformation;
using AutoMapper;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.Application.MappingProfiles;

public class MonitorMappingProfile : Profile
{
    public MonitorMappingProfile()
    {
        CreateMap<AddMonitorDto, Monitor>();
        
        CreateMap<Monitor, MonitorDto>()
            .ForMember(dest => dest.CheckIntervalSeconds, opt => opt.MapFrom(src => src.IntervalSeconds))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Target))
            .ReverseMap();

        CreateMap<UpdateMonitorDto, Monitor>()
            .ForMember(dest => dest.Target, opt => opt.MapFrom(src => src.Url));
        
        CreateMap<UpdateMonitorDto, MonitorDto>();
    }
}