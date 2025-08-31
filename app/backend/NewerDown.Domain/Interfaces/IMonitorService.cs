using Microsoft.AspNetCore.Http;
using NewerDown.Domain.DTOs.Service;
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
}