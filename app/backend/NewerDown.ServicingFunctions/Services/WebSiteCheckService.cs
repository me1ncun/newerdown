using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.DTOs.Notification;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;
using NewerDown.Infrastructure.Data;
using NewerDown.Infrastructure.Extensions;
using NewerDown.Infrastructure.Queuing;

namespace NewerDown.ServicingFunctions.Services
{
    public interface IWebSiteCheckService
    {
        Task CheckWebsiteAsync(MonitorDto request, CancellationToken cancellationToken);
    }

    public class WebSiteCheckService : IWebSiteCheckService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WebSiteCheckService> _logger;
        private readonly IQueueSender _queueSender;

        public WebSiteCheckService(
            ApplicationDbContext context,
            IHttpClientFactory httpClientFactory,
            ILogger<WebSiteCheckService> logger,
            IQueueSenderFactory queueSenderFactory)

        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _queueSender = queueSenderFactory.Create(QueueType.Notifications.GetQueueName());
        }

        public async Task CheckWebsiteAsync(MonitorDto request, CancellationToken cancellationToken)
        {
            bool isSuccess = false;
            var monitor = await _context.Monitors
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken: cancellationToken);

            if (monitor is null)
            {
                _logger.LogWarning("Monitor {MonitorId} not found", request.Id);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            HttpResponseMessage? response = null;
            string? error = null;

            try
            {
                var client = _httpClientFactory.CreateClient("MonitorClient");
                response = await client.GetAsync(monitor?.Target, cancellationToken);
                stopwatch.Stop();
                isSuccess = response.IsSuccessStatusCode;
                _logger.LogDebug("Monitor {MonitorId} responded {StatusCode} in {Elapsed}ms", monitor.Id, (int)response.StatusCode, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                error = ex.ToString();
                _logger.LogError(ex, "Error checking monitor {MonitorId}", monitor.Id);
            }

            var check = new MonitorCheck
            {
                Id = Guid.NewGuid(),
                MonitorId = monitor.Id,
                CheckedAt = DateTime.UtcNow,
                ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
                StatusCode = response?.StatusCode.ToString(),
                IsSuccess = isSuccess,
                ErrorMessage = error
            };

            _context.MonitorChecks.Add(check);

            Alert? alert = null;
            if (!isSuccess)
            {
                alert = new Alert
                {
                    Id = Guid.NewGuid(),
                    MonitorId = monitor.Id,
                    CreatedAt = DateTime.UtcNow,
                    Message = error ?? $"Unexpected status code {(int?)response?.StatusCode}"
                };
                _context.Alerts.Add(alert);
            }
       
            await _context.SaveChangesAsync(cancellationToken);
            
            if (!isSuccess && alert is not null)
            {
                var notification = new NotificationDto
                {
                    UserName = monitor.User.UserName,
                    Email = monitor.User.Email,
                    AlertType = alert.Type,
                    Target = monitor.Target,
                    Message = alert.Message
                };
                try
                {
                    await _queueSender.SendAsync(notification);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send notification for monitor {MonitorId}", monitor.Id);
                }
            }
            
            _logger.LogInformation("Website check for monitor {MonitorId} completed, success={Success}", monitor.Id, isSuccess);
        }
    }
}
