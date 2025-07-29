using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IMonitorService
{
    Task<IEnumerable<MonitorDto>> GetAllMonitors();
    Task<MonitorDto> GetMonitorByIdAsync(Guid id);
    Task<Guid> CreateMonitorAsync(AddMonitorDto monitorDto);
    Task UpdateMonitorAsync(Guid serviceId, UpdateMonitorDto monitorDto);
    Task DeleteMonitorAsync(Guid id);
}