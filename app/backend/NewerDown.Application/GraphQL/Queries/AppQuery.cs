using GraphQL;
using GraphQL.Types;
using NewerDown.Application.Extensions;
using NewerDown.Application.GraphQL.Types;
using NewerDown.Domain.DTOs.Incidents;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application.GraphQL.Queries;

public class AppQuery : ObjectGraphType
{
    public AppQuery(IIncidentService incidentService)
    {
        Field<ListGraphType<IncidentDtoType>>("incidents")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "userId" }
            ))
            .ResolveAsync(GraphQLCustomExtensions.SafeResolve(async context =>
            {
                var userId = context.GetArgument<Guid>("userId");
                return await incidentService.GetAllAsync(userId);
            })!);
        
        Field<IncidentDtoType>("incident")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "userId" }
            ))
            .ResolveAsync(GraphQLCustomExtensions.SafeResolve(async context =>
            {
                var id = context.GetArgument<Guid>("id");
                var userId = context.GetArgument<Guid>("userId");
                return await incidentService.GetByIdAsync(id, userId);
            })!);
    }
}