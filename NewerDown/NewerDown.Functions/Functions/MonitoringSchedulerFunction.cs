using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewerDown.Functions.Models;
using NewerDown.Infrastructure.Data;
using Newtonsoft.Json;

namespace NewerDown.Functions.Functions;

public class MonitoringSchedulerFunction
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MonitoringSchedulerFunction> _logger;

    public MonitoringSchedulerFunction(ApplicationDbContext context,
        ILogger<MonitoringSchedulerFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function(nameof(MonitoringSchedulerFunction))]
    [ServiceBusOutput("monitoring", Connection = "ServiceBusConnection")]
    public async Task<IEnumerable<string>> Run([TimerTrigger("0 */1 * * * *")] TimerInfo timer)
    {
        var services = await _context.Services
            .Where(s => s.IsActive && s.CheckIntervalSeconds <= 60)
            .ToListAsync();

        return services.Select(service =>
            {
                var json = JsonConvert.SerializeObject(new ServiceDto() { ServiceId = service.Id });
                _logger.LogInformation($"Sending message: {json}");
                return json;
            }
        ).ToArray();
    }
}