using CsvHelper.Configuration;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.CsvProfiles;

public sealed class AddMonitorDtoProfile : ClassMap<AddMonitorDto>
{
    public AddMonitorDtoProfile()
    {
        Map(m => m.Name).Index(0).Name("Name");
        Map(m => m.Target).Index(1).Name("Url");
        Map(m => m.Type).Index(2).Name("Type");
        Map(m => m.IntervalSeconds).Index(3).Name("CheckIntervalSeconds");
        Map(m => m.IsActive).Index(4).Name("IsActive");
    }
}
