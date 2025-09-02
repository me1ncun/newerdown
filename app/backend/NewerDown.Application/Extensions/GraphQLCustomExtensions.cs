using GraphQL;

namespace NewerDown.Application.Extensions;

public static class GraphQLCustomExtensions
{
    public static Func<IResolveFieldContext<object>, Task<object?>> SafeResolve<T>(Func<IResolveFieldContext, Task<T>> func)
    {
        return async context =>
        {
            try
            {
                return await func(context);
            }
            catch (Exception ex)
            {
                context.Errors.Add(new ExecutionError(ex.Message));
                return default(T);
            }
        };
    }
}