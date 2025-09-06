using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.Entities;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Functions.Services;

public interface IStatisticsService
{
    Task CalculateStatisticsAsync();
}

public class StatisticsService : IStatisticsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StatisticsService> _logger;

    public StatisticsService(ApplicationDbContext context, ILogger<StatisticsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CalculateStatisticsAsync()
    {
        var now = DateTime.UtcNow;
        var periodStart = now.AddHours(-1);

        _logger.LogInformation("Calculating monitor statistics for {PeriodStart} - {Now}", periodStart, now);

        var checks = await _context.MonitorChecks
            .Where(c => c.CheckedAt >= periodStart && c.CheckedAt < now)
            .ToListAsync();

        var grouped = checks.GroupBy(c => c.MonitorId);

        var stats = new List<MonitorStatistic>();

        foreach (var group in grouped)
        {
            var total = group.Count();
            var failed = group.Count(c => !c.IsSuccess);
            var uptime = total == 0 ? 0 : ((double)(total - failed) / total) * 100.0;
            var avgResponse = group.Any() ? group.Average(c => c.ResponseTimeMs) : 0;

            var incidents = 0;
            var prevFail = false;
            foreach (var check in group.OrderBy(c => c.CheckedAt))
            {
                if (!check.IsSuccess && !prevFail)
                {
                    incidents++;
                    prevFail = true;
                }
                else if (check.IsSuccess)
                {
                    prevFail = false;
                }
            }

            stats.Add(new MonitorStatistic
            {
                Id = Guid.NewGuid(),
                MonitorId = group.Key,
                PeriodStart = periodStart,
                PeriodEnd = now,
                UptimePercent = uptime,
                AvgResponseTimeMs = avgResponse.Value,
                TotalChecks = total,
                FailedChecks = failed,
                IncidentsCount = incidents
            });
        }

        if (stats.Any())
        {
            _context.MonitorStatistics.AddRange(stats);
            await _context.SaveChangesAsync();
        }

        _logger.LogInformation("Saved {Count} monitor statistics records", stats.Count);
    }
}