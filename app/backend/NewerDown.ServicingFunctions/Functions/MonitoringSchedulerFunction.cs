using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Infrastructure.Data;

namespace NewerDown.ServicingFunctions.Functions;

public class MonitoringSchedulerFunction
{
    private readonly ILogger _logger;
    private readonly ApplicationDbContext _context;

    public MonitoringSchedulerFunction(ILoggerFactory loggerFactory, ApplicationDbContext context)
    {
        _logger = loggerFactory.CreateLogger<MonitoringSchedulerFunction>();
        _context = context;
    }

    [Function("MonitoringSchedulerFunction")]
    [ServiceBusOutput("monitoring", Connection = "ServiceBusConnection")]
    public async Task<IEnumerable<string>> Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        var now = DateTime.UtcNow;

        var monitors = await _context.Monitors
            .Where(m => m.IsActive)
            .Include(m => m.Checks.OrderByDescending(c => c.CheckedAt).Take(1))
            .ToListAsync();

        var messages = new List<string>();
        foreach (var monitor in monitors)
        {
            var lastCheck = monitor.Checks.FirstOrDefault();
            var shouldCheck = lastCheck == null || (now - lastCheck.CheckedAt).TotalSeconds >= monitor.IntervalSeconds;
            if (shouldCheck)
            {
                var dto = new MonitorDto { Id = monitor.Id, Url = monitor.Target };
                var json = JsonSerializer.Serialize(dto);

                _logger.LogInformation("Scheduling check for monitor {MonitorId} at {Now}", monitor.Id, now);
                messages.Add(json);
            }
        }

        return messages;
    }
}