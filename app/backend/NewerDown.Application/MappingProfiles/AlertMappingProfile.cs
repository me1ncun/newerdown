using AutoMapper;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class AlertMappingProfile : Profile
{
    public AlertMappingProfile()
    {
        CreateMap<AddAlertDto, Alert>();
    }
}