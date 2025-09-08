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
    public interface IMonitorService
    {
        Task<bool> CheckWebsiteAsync(MonitorDto dto);
    }

    public class MonitorService : IMonitorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MonitorService> _logger;
        private readonly IQueueSender _queueSender;

        public MonitorService(
            ApplicationDbContext context,
            IHttpClientFactory httpClientFactory,
            ILogger<MonitorService> logger,
            IQueueSenderFactory queueSenderFactory)

        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _queueSender = queueSenderFactory.Create(QueueType.Notifications.GetQueueName());
        }

        public async Task<bool> CheckWebsiteAsync(MonitorDto dto)
        {
            var monitor = await _context.Monitors
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (monitor is null)
            {
                _logger.LogWarning("Monitor {MonitorId} not found", dto.Id);
                return false;
            }

            var stopwatch = Stopwatch.StartNew();
            HttpResponseMessage? response = null;
            bool isSuccess = false;
            string? error = null;

            try
            {
                response = await _httpClientFactory.CreateClient().GetAsync(monitor.Target);
                stopwatch.Stop();
                isSuccess = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                error = ex.Message;
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

            if (!isSuccess)
            {
                var alert = new Alert
                {
                    Id = Guid.NewGuid(),
                    MonitorId = monitor.Id,
                    CreatedAt = DateTime.UtcNow,
                    Message = error ?? $"Unexpected status code {(int?)response?.StatusCode}"
                };

                _context.Alerts.Add(alert);

                await _queueSender.SendAsync(new NotificationDto()
                {
                    UserName = monitor.User.UserName,
                    Email = monitor.User.Email,
                    AlertType = alert.Type,
                    Target = monitor.Target,
                    Message = alert.Message
                });
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Website check completed for monitor {MonitorId}, success={Success}", monitor.Id, isSuccess);

            return isSuccess;
        }
    }
}
