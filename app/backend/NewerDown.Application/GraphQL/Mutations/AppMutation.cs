using GraphQL;
using GraphQL.Types;
using NewerDown.Application.Extensions;
using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.Incidents;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application.GraphQL.Mutations;

public class AppMutation : ObjectGraphType
{
    public AppMutation(IIncidentService incidentService, IScopedTimeProvider timeProvider)
    {
        Field<BooleanGraphType>("acknowledgeIncident")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "userId" }
            ))
            .ResolveAsync(GraphQLCustomExtensions.SafeResolve(async context =>
            {
                var id = context.GetArgument<Guid>("id");
                var userId = context.GetArgument<Guid>("userId");
                await incidentService.AcknowledgeIncidentAsync(id, userId);
                return true;
            })!);

        Field<BooleanGraphType>("commentIncident")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "comment" },
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "incidentId" },
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "userId" }
            ))
            .ResolveAsync(GraphQLCustomExtensions.SafeResolve(async context =>
                {
                    var commentText = context.GetArgument<string>("comment");
                    var incidentId = context.GetArgument<Guid>("incidentId");
                    var userId = context.GetArgument<Guid>("userId");
                    
                    var comment = new CreateIncidentCommentDto()
                    {
                        IncidentId = incidentId,
                        Comment = commentText,
                        CreatedAt = timeProvider.UtcNow(),
                        UserId = userId
                    };

                    await incidentService.CommentIncidentAsync(comment);
                    return true;
                }
            )!);
    }
}