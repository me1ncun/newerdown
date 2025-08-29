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
            .ReverseMap();

        CreateMap<UpdateMonitorDto, Monitor>();
        
        CreateMap<UpdateMonitorDto, MonitorDto>();
    }
}