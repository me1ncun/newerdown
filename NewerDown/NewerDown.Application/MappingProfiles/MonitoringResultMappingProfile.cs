using AutoMapper;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class MonitoringResultMappingProfile : Profile
{
    public MonitoringResultMappingProfile()
    {
        CreateMap<MonitoringResult, MonitoringResultDto>();
    }
}