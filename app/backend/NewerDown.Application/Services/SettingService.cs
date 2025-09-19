using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NewerDown.Domain.DTOs.MonitorCheck;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class SettingService : ISettingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SettingService> _logger;

    public SettingService(
        ApplicationDbContext context,
        ILogger<SettingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<MonitorTypeDto>> GetCheckTypesAsync()
    {
        return await _context.Monitors
            .Select(m => new MonitorTypeDto
            {
                Id = m.Id,
                Type = m.Type
            })
            .ToListAsync();
    }

    public async Task<List<MonitorIntervalDto>> GetMonitorIntervalsAsync()
    {
        return await _context.Monitors
            .Select(m => new MonitorIntervalDto()
            {
                Id = m.Id,
                Interval = m.IntervalSeconds
            })
            .ToListAsync();
    }
    
    public async Task<List<string?>> GetStatusCodesAsync() 
    {
         return await _context.MonitorChecks.Select(mc => mc.StatusCode).Distinct().ToListAsync();
    }
}