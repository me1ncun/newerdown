using AutoMapper;
using NewerDown.Application.Resolvers;
using NewerDown.Domain.Paging;

namespace NewerDown.Application.MappingProfiles;

public class GeneralMappingProfile : Profile
{
    public GeneralMappingProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));
    }
}