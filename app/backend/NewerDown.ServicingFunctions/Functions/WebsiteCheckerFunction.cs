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
    private readonly IWebSiteCheckService _webSiteCheckService;

    public WebsiteCheckerFunction(ILogger<WebsiteCheckerFunction> logger, IWebSiteCheckService webSiteCheckService)
    {
        _logger = logger;
        _webSiteCheckService = webSiteCheckService;
    }

    [Function(nameof(WebsiteCheckerFunction))]
    public async Task Run(
        [ServiceBusTrigger("monitoring", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var monitorDto = JsonSerializer.Deserialize<MonitorDto>(Encoding.UTF8.GetString(message.Body)) ??
                         throw new InvalidOperationException("Invalid monitor message");
        
        var result = await _webSiteCheckService.CheckWebsiteAsync(monitorDto);
        _logger.LogInformation("Website check completed for monitor {MonitorId}, success={Success}", monitorDto.Id, result);
    }
}