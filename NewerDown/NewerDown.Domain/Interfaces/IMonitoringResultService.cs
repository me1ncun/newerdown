using NewerDown.Domain.DTOs.MonitoringResults;

namespace NewerDown.Domain.Interfaces;

public interface IMonitoringResultService
{
    Task<IEnumerable<MonitoringResultDto>> GetMonitoringResultsAsync(string? filter, int page, int pageSize);
    Task<IEnumerable<MonitoringResultDto>> GetMonitoringResultsByDaysAsync(int days);
}