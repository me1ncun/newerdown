using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NewerDown.ServicingFunctions.Services;

namespace NewerDown.ServicingFunctions.Functions;

public class CalculateStatisticFunction
{
    private readonly ILogger _logger;
    private readonly IStatisticsService _statisticsService;

    public CalculateStatisticFunction(ILoggerFactory loggerFactory, IStatisticsService statisticsService)
    {
        _logger = loggerFactory.CreateLogger<CalculateStatisticFunction>();
        _statisticsService = statisticsService;
    }

    [Function("CalculateStatisticFunction")]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer)
    {
        await _statisticsService.CalculateStatisticsAsync();
        _logger.LogInformation("Statistics calculation completed at: {Time}", DateTime.UtcNow);
    }
}