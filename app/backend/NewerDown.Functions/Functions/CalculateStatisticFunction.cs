using Microsoft.Azure.Functions.Worker;
using NewerDown.Functions.Services;

namespace NewerDown.Functions.Functions;

public class CalculateStatisticFunction
{
    private readonly IStatisticsService _statisticsService;

    public CalculateStatisticFunction(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [Function(nameof(CalculateStatisticFunction))]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo timer) 
    {
        await _statisticsService.CalculateStatisticsAsync();
    }
}