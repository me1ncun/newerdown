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
        var req = Encoding.UTF8.GetString(message.Body);
        var monitor = JsonSerializer.Deserialize<MonitorDto>(req) ??
                         throw new InvalidOperationException("Invalid monitor message");
        
        await _webSiteCheckService.CheckWebsiteAsync(monitor, CancellationToken.None);
    }
}