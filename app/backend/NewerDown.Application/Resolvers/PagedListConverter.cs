using AutoMapper;
using NewerDown.Domain.Paging;

namespace NewerDown.Application.Resolvers;

public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
{
    public PagedList<TDestination> Convert(
        PagedList<TSource> source, 
        PagedList<TDestination> destination, 
        ResolutionContext context)
    {
        var items = context.Mapper.Map<IEnumerable<TDestination>>(source.Items);

        return new PagedList<TDestination>(
            items,
            source.TotalCount,
            source.CurrentPage,
            source.PageSize
        );
    }
}