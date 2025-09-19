using NewerDown.Domain.DTOs.MonitorCheck;

namespace NewerDown.Domain.Interfaces;

public interface ISettingService
{
    Task<List<MonitorTypeDto>> GetCheckTypesAsync();
    Task<List<MonitorIntervalDto>> GetMonitorIntervalsAsync();
    Task<List<string?>> GetStatusCodesAsync();
}