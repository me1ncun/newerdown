using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.DTOs.Service;
using NewerDown.ServicingFunctions.Services;

namespace NewerDown.ServicingFunctions.Functions;

public class WebsiteCheckerFunction
{
    private readonly ILogger<WebsiteCheckerFunction> _logger;
    private readonly IMonitorService _monitorService;

    public WebsiteCheckerFunction(ILogger<WebsiteCheckerFunction> logger, IMonitorService monitorService)
    {
        _logger = logger;
        _monitorService = monitorService;
    }

    [Function(nameof(WebsiteCheckerFunction))]
    public async Task Run(
        [ServiceBusTrigger("monitoring", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var monitorDto = JsonSerializer.Deserialize<MonitorDto>(Encoding.UTF8.GetString(message.Body)) ??
                         throw new InvalidOperationException("Invalid monitor message");
        
        var result = await _monitorService.CheckWebsiteAsync(monitorDto);
        _logger.LogInformation("Website check completed for monitor {MonitorId}, success={Success}", monitorDto.Id, result);
    }
}