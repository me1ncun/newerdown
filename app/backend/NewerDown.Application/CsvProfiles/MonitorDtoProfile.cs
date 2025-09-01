using CsvHelper.Configuration;
using NewerDown.Domain.DTOs.Service;

namespace NewerDown.Application.CsvProfiles;

public sealed class MonitorDtoProfile : ClassMap<MonitorDto>
{
    public MonitorDtoProfile()
    {
        Map(m => m.Id).Index(0).Name("Id");
        Map(m => m.Name).Index(1).Name("Name");
        Map(m => m.Url).Index(2).Name("Url");
        Map(m => m.CheckIntervalSeconds).Index(2).Name("CheckIntervalSeconds");
        Map(m => m.IsActive).Index(3).Name("IsActive");
        Map(m => m.CreatedAt).Index(4).Name("CreatedAt");
    }
}
