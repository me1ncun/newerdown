using GraphQL.Types;
using NewerDown.Domain.DTOs.Incidents;

namespace NewerDown.Application.GraphQL.Types;

public class IncidentDtoType : ObjectGraphType<IncidentDto>
{
    public IncidentDtoType()
    {
        Field(x => x.Id, type: typeof(IdGraphType));
        Field(x => x.MonitorId);
        Field(x => x.StartedAt);
        Field(x => x.ResolvedAt, nullable: true);
        Field(x => x.RootCause, nullable: true);
        Field(x => x.ResolutionComment, nullable: true);
        Field(x => x.IsAcknowledged);
    }
}