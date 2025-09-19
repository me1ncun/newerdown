using Microsoft.AspNetCore.Http;
using NewerDown.Domain.DTOs.MonitorCheck;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.DTOs.Request;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Paging;
using NewerDown.Domain.Result;

namespace NewerDown.Domain.Interfaces;

public interface IMonitorService
{
    Task<List<MonitorDto>> GetAllMonitorsAsync();
    Task<Result<MonitorDto>> GetMonitorByIdAsync(GetByIdDto request);
    Task<Result<Guid>> CreateMonitorAsync(AddMonitorDto request);
    Task<Result.Result> UpdateMonitorAsync(Guid monitorId, UpdateMonitorDto request);
    Task<Result.Result> DeleteMonitorAsync(DeleteMonitorDto request);
    Task<Result.Result> PauseMonitorAsync(GetByIdDto request);
    Task<Result.Result> ResumeMonitorAsync(GetByIdDto request);
    Task<byte[]> ExportMonitorCsvAsync(GetByIdDto request);
    Task ImportMonitorsFromCsvAsync(IFormFile file);
    Task<MonitorStatus> GetMonitorStatusAsync(GetByIdDto request);
    Task<PagedList<MonitorCheckDto>> GetHistoryByMonitorAsync(Guid id, int pageNumber = 1, int pageSize = 30);
    Task<List<MonitorCheckShortDto>> GetLatencyGraph(Guid id, DateTime from, DateTime to);
    Task<UptimePercentageDto> GetUptimePercentageAsync(Guid id, DateTime from, DateTime to);
    Task<List<DownTimeDto>> GetDownTimesAsync(GetByIdDto request);
    Task<MonitorSummaryDto> GetMonitorSummaryAsync(Guid id, int hours);
}