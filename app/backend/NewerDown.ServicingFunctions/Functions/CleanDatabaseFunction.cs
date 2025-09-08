using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewerDown.Infrastructure.Data;

namespace NewerDown.ServicingFunctions.Functions;

public class CleanDatabaseFunction
{
    private readonly ILogger _logger;
    private readonly ApplicationDbContext _dbContext;

    public CleanDatabaseFunction(ILoggerFactory loggerFactory, ApplicationDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger<CleanDatabaseFunction>();
        _dbContext = dbContext;
    }

    [Function("CleanDatabaseFunction")]
    public async Task Run([TimerTrigger("0 0 3 * * *")] TimerInfo myTimer)
    {
        var cutoffQuarterDate = DateTime.UtcNow.AddDays(-30);
        var cutoffOldDate = DateTime.UtcNow.AddDays(-90);

        var deletedMonitorChecks = await _dbContext.MonitorChecks.Where(mc => mc.CheckedAt < cutoffOldDate).ExecuteDeleteAsync();
        var deletedIncidents = await _dbContext.Incidents.Where(i => i.ResolvedAt < cutoffQuarterDate).ExecuteDeleteAsync();

        _logger.LogInformation(
            "Cleaned up {MonitorChecks} monitor checks, {Incidents} incidents",
            deletedMonitorChecks,
            deletedIncidents);
    }
}