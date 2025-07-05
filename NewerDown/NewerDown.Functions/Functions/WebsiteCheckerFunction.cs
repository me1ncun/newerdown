using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.Interfaces;
using NewerDown.Functions.Models;
using NewerDown.Functions.Services;
using NewerDown.Infrastructure.Data;
using Newtonsoft.Json;

namespace NewerDown.Functions.Functions;

public class WebsiteCheckerFunction
{
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IWebsiteCheckerService _websiteCheckerService;
    private readonly ILogger<WebsiteCheckerFunction> _logger;

    public WebsiteCheckerFunction(
        ApplicationDbContext context,
        INotificationService notificationService,
        IWebsiteCheckerService websiteCheckerService,
        ILogger<WebsiteCheckerFunction> logger)
    {
        _context = context;
        _notificationService = notificationService;
        _websiteCheckerService = websiteCheckerService;
        _logger = logger;
    }

    [Function(nameof(WebsiteCheckerFunction))]
    public async Task Run(
        [ServiceBusTrigger("monitoring", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var data = JsonConvert.DeserializeObject<ServiceDto>(Encoding.UTF8.GetString(message.Body));
        Guid serviceId = data.ServiceId;

        var result = await _websiteCheckerService.CheckWebsiteAsync(serviceId);

        _context.MonitoringResults.Add(result);
        await _context.SaveChangesAsync();

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}