using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Application.GraphQL.Mutations;
using NewerDown.Application.GraphQL.Queries;

namespace NewerDown.Application.GraphQL.Schemas;

public class AppSchema : Schema
{
    public AppSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<AppQuery>();
        Mutation = provider.GetRequiredService<AppMutation>();
    }
}