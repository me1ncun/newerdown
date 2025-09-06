using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Functions.Services;

namespace NewerDown.Functions.Functions;

public class WebsiteCheckerFunction
{
    private readonly IMonitorService _monitorService;
    private readonly ILogger<WebsiteCheckerFunction> _logger;

    public WebsiteCheckerFunction(
        IMonitorService monitorService,
        ILogger<WebsiteCheckerFunction> logger)
    {
        _monitorService = monitorService;
        _logger = logger;
    }

    [Function(nameof(WebsiteCheckerFunction))]
    public async Task Run(
        [ServiceBusTrigger("monitoring", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message, 
        ServiceBusMessageActions messageActions)
    {
        var monitorDto = JsonSerializer.Deserialize<MonitorDto>(Encoding.UTF8.GetString(message.Body)) 
                         ?? throw new InvalidOperationException("Invalid monitor message");;

        var result = await _monitorService.CheckWebsiteAsync(monitorDto);

        _logger.LogInformation("Website check completed for monitor {MonitorId}, success={Success}", monitorDto.Id, result);
    }
}
