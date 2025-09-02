using GraphQL.Types;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.GraphQL.Types;

public class IncidentType : ObjectGraphType<Incident>
{
    public IncidentType()
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("Id property from the incident object.");
        Field(x => x.MonitorId).Description("Monitor Id property from the incident object.");
        Field(x => x.StartedAt).Description("StartedAt property from the incident object.");
        Field(x => x.ResolvedAt).Description("ResolvedAt property from the incident object.");
        Field(x => x.RootCause).Description("RootCause property from the incident object.");
        Field(x => x.ResolutionComment).Description("ResolutionComment property from the incident object.");
        Field(x => x.IsAcknowledged).Description("IsAcknowledged property from the incident object.");
    }
}