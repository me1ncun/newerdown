using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.DTOs.Notification;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Interfaces;
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
            string? statusCode = null;
            string? error = null;

            try
            {
                switch (monitor.Type)
                {
                    case MonitorType.Http:
                        (isSuccess, statusCode, error) = await CheckHttpAsync(monitor.Target, cancellationToken);
                        break;

                    case MonitorType.Tcp:
                        (isSuccess, statusCode, error) = await CheckTcpAsync(monitor.Target, monitor.Port.Value);
                        break;

                    case MonitorType.Ping:
                        (isSuccess, statusCode, error) = await CheckPingAsync(monitor.Target);
                        break;
                }
                
                
                var client = _httpClientFactory.CreateClient("MonitorClient");
                var response = await client.GetAsync(monitor?.Target, cancellationToken);
                stopwatch.Stop();
                isSuccess = response.IsSuccessStatusCode;
                _logger.LogDebug("Monitor {MonitorId} responded {StatusCode} in {Elapsed}ms", monitor.Id, statusCode, stopwatch.ElapsedMilliseconds);
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
                StatusCode = statusCode,
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
                    Message = error ?? $"Unexpected status code {statusCode}"
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
                
                await _queueSender.SendAsync(notification);
            }
            
            _logger.LogInformation("Website check for monitor {MonitorId} completed, success={Success}", monitor.Id, isSuccess);
        }
        
        private async Task<(bool success, string? status, string? error)> CheckHttpAsync(string url, CancellationToken ct)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MonitorClient");
                var response = await client.GetAsync(url, ct);
                return (response.IsSuccessStatusCode, ((int)response.StatusCode).ToString(), null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        
        private async Task<(bool success, string? status, string? error)> CheckTcpAsync(string host, int port)
        {
            try
            {
                using var client = new TcpClient();
                var connectTask = client.ConnectAsync(host, port);
                var timeoutTask = Task.Delay(5000); 
                var finished = await Task.WhenAny(connectTask, timeoutTask);

                if (finished == timeoutTask)
                    return (false, null, "TCP connection timeout");

                return (client.Connected, "TCP Connected", null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        
        private async Task<(bool success, string? status, string? error)> CheckPingAsync(string host)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(host, 5000); 
                if (reply.Status == IPStatus.Success)
                {
                    return (true, "Ping Success", null);
                }
                return (false, reply.Status.ToString(), null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}
