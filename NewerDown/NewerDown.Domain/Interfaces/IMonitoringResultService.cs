using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.Paging;

namespace NewerDown.Domain.Interfaces;

public interface IMonitoringResultService
{
    Task<PagedResponse<MonitoringResultDto>> GetMonitoringResultsAsync(string? filter, int page, int pageSize);
    Task<IEnumerable<MonitoringResultDto>> GetMonitoringResultsByDaysAsync(int days);
}