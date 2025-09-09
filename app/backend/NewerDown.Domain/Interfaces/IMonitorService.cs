using Microsoft.AspNetCore.Http;
using NewerDown.Domain.DTOs.MonitorCheck;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Paging;
using NewerDown.Domain.Result;

namespace NewerDown.Domain.Interfaces;

public interface IMonitorService
{
    Task<IEnumerable<MonitorDto>> GetAllMonitors();
    Task<Result<MonitorDto>> GetMonitorByIdAsync(Guid id);
    Task<Result<Guid>> CreateMonitorAsync(AddMonitorDto monitorDto);
    Task<Result.Result> UpdateMonitorAsync(Guid serviceId, UpdateMonitorDto monitorDto);
    Task<Result.Result> DeleteMonitorAsync(Guid id);
    Task<Result.Result> PauseMonitorAsync(Guid id);
    Task<Result.Result> ResumeMonitorAsync(Guid id);
    Task<byte[]> ExportMonitorCsvAsync(Guid id);
    Task ImportMonitorsFromCsvAsync(IFormFile file);
    Task<MonitorStatus> GetMonitorStatusAsync(Guid id);
    Task<PagedList<MonitorCheckDto>> GetHistoryByMonitorAsync(Guid id, int pageNumber = 1, int pageSize = 30);
    Task<List<MonitorCheckShortDto>> GetLatencyGraph(Guid id, DateTime from, DateTime to);
    Task<UptimePercentageDto> GetUptimePercentageAsync(Guid id, DateTime from, DateTime to);
    Task<List<DownTimeDto>> GetDownTimesAsync(Guid id);
    Task<MonitorSummaryDto> GetMonitorSummaryAsync(Guid id, int hours);
}