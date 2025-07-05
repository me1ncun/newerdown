using System.Diagnostics;
using NewerDown.Domain.Entities;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Functions.Services;

public interface IWebsiteCheckerService
{
    Task CheckWebsiteAsync(Guid serviceId);
}

public class WebsiteCheckerService : IWebsiteCheckerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApplicationDbContext _context;

    public WebsiteCheckerService(
        IHttpClientFactory httpClientFactory,
        ApplicationDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    public async Task CheckWebsiteAsync(Guid serviceId)
    {
        var result = new MonitoringResult
        {
            Id = Guid.NewGuid(),
            ServiceId = serviceId,
            CheckedAt = DateTime.UtcNow
        };

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            
            var service = await _context.Services.FindAsync(serviceId);

            var response = await client.GetAsync(service.Url);

            stopwatch.Stop();

            result.StatusCode = (int)response.StatusCode;
            result.ResponseTimeMs = stopwatch.Elapsed.TotalMilliseconds;
            result.IsAlive = response.IsSuccessStatusCode;
            result.Error = response.IsSuccessStatusCode ? null : $"HTTP Error: {response.StatusCode}";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            result.StatusCode = 0;
            result.ResponseTimeMs = stopwatch.Elapsed.TotalMilliseconds;
            result.IsAlive = false;
            result.Error = ex.Message;
        }
        
        _context.MonitoringResults.Add(result);
        await _context.SaveChangesAsync();
    }
}