using AutoMapper;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class MonitorCheckMappingProfile : Profile
{
    public MonitorCheckMappingProfile()
    {
        CreateMap<MonitorCheck, MonitoringResultDto>();
    }
}