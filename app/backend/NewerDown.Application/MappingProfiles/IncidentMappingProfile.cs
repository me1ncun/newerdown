using AutoMapper;
using NewerDown.Domain.DTOs.Incidents;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class IncidentMappingProfile : Profile
{
    public IncidentMappingProfile()
    {
        CreateMap<Incident, IncidentDto>();
        
        CreateMap<CreateIncidentCommentDto, IncidentComment>();
    }
}