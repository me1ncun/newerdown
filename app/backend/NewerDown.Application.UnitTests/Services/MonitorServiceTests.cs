using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;
using System.Net;
using System.Text;
using AutoMapper;
using NewerDown.Application.Time;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class MonitorServiceTests
{
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<IUserContextService> _userContextServiceMock;
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<IScopedTimeProvider> _timeProviderMock;
    private ApplicationDbContext _context;
    private MonitorService _monitorService;
    private IMapper _mapper;
    private readonly Guid _currentUserId = Guid.NewGuid();

    [SetUp]
    public void Setup()
    {
        _cacheServiceMock = new();
        _userContextServiceMock = new();
        _httpClientFactoryMock = new();
        _timeProviderMock = new();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        
        _cacheServiceMock.Invocations.Clear(); 
        _cacheServiceMock.Reset();

        _mapper = new MapperConfiguration(cfg => { 
            cfg.AddProfiles(new List<Profile>
            {
                new MonitorMappingProfile(),
                new GeneralMappingProfile(),
                new MonitorCheckMappingProfile()
            });
        }).CreateMapper();

        _userContextServiceMock.Setup(x => x.GetUserId()).Returns(_currentUserId);
        _timeProviderMock.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

        var httpClient = new HttpClient(new HttpMessageHandlerStub());
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _monitorService = new MonitorService(
            _context,
            _mapper,
            _cacheServiceMock.Object,
            _userContextServiceMock.Object,
            _httpClientFactoryMock.Object,
            Mock.Of<Microsoft.Extensions.Logging.ILogger<MonitorService>>(),
            _timeProviderMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task CreateMonitorAsync_ShouldCreateMonitor()
    {
        var dto = new AddMonitorDto
        {
            Name = "Monitor1",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };

        var result = await _monitorService.CreateMonitorAsync(dto);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(await _context.Monitors.AnyAsync(m => m.Name == "Monitor1"), Is.True);
    }

    [Test]
    public async Task GetMonitorByIdAsync_ShouldReturnMonitor()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "Monitor2",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var result = await _monitorService.GetMonitorByIdAsync(monitor.Id);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Name, Is.EqualTo("Monitor2"));
    }

    [Test]
    public async Task UpdateMonitorAsync_ShouldUpdateMonitor()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "Monitor3",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var dto = new UpdateMonitorDto
        {
            Name = "Monitor3Updated",
            Url = "https://updated.com",
            Type = MonitorType.Http,
            IsActive = false
        };

        var result = await _monitorService.UpdateMonitorAsync(monitor.Id, dto);

        Assert.That(result.IsSuccess, Is.True);
        var updated = await _context.Monitors.FindAsync(monitor.Id);
        Assert.That(updated.Name, Is.EqualTo("Monitor3Updated"));
        Assert.That(updated.Target, Is.EqualTo("https://updated.com"));
        Assert.That(updated.IsActive, Is.False);
    }

    [Test]
    public async Task DeleteMonitorAsync_ShouldDeleteMonitor()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "Monitor4",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var result = await _monitorService.DeleteMonitorAsync(monitor.Id);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(await _context.Monitors.FindAsync(monitor.Id), Is.Null);
    }

    [Test]
    public async Task PauseMonitorAsync_ShouldPauseMonitor()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "Monitor5",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var result = await _monitorService.PauseMonitorAsync(monitor.Id);

        Assert.That(result.IsSuccess, Is.True);
        var paused = await _context.Monitors.FindAsync(monitor.Id);
        Assert.That(paused.IsActive, Is.False);
    }

    [Test]
    public async Task ResumeMonitorAsync_ShouldResumeMonitor()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "Monitor6",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = false
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var result = await _monitorService.ResumeMonitorAsync(monitor.Id);

        Assert.That(result.IsSuccess, Is.True);
        var resumed = await _context.Monitors.FindAsync(monitor.Id);
        Assert.That(resumed.IsActive, Is.True);
    }

    [Test]
    public async Task GetAllMonitors_ShouldReturnMonitors()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "MonitorAll",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var result = await _monitorService.GetAllMonitors();

        Assert.That(result.Any(m => m.Name == "MonitorAll"), Is.True);
    }

    [Test]
    public async Task ExportMonitorCsvAsync_ShouldReturnCsvBytes()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "MonitorCsv",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var bytes = await _monitorService.ExportMonitorCsvAsync(monitor.Id);

        Assert.That(bytes.Length, Is.GreaterThan(0));
    }

    [Test]
    public async Task ImportMonitorsFromCsvAsync_ShouldImportMonitors()
    {
        var csv = "Name,Url,Type,CheckIntervalSeconds,IsActive\nMonitorCsvImport,https://import.com,Http,60,true";
        var fileMock = new Mock<Microsoft.AspNetCore.Http.IFormFile>();
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

        await _monitorService.ImportMonitorsFromCsvAsync(fileMock.Object);

        Assert.That(await _context.Monitors.AnyAsync(m => m.Name == "MonitorCsvImport"), Is.True);
    }

    [Test]
    public async Task GetMonitorStatusAsync_ShouldReturnUpOrDown()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "MonitorStatus",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true,
            Checks = new List<MonitorCheck>
            {
                new MonitorCheck { CheckedAt = DateTime.UtcNow, IsSuccess = true }
            }
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var status = await _monitorService.GetMonitorStatusAsync(monitor.Id);

        Assert.That(status, Is.EqualTo(MonitorStatus.Up));
    }

    [Test]
    public async Task GetHistoryByMonitorAsync_ShouldReturnPagedChecks()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "MonitorHistory",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var check = new MonitorCheck
        {
            Id = Guid.NewGuid(),
            MonitorId = monitor.Id,
            CheckedAt = DateTime.UtcNow,
            IsSuccess = true
        };
        await _context.MonitorChecks.AddAsync(check);
        await _context.SaveChangesAsync();

        var paged = await _monitorService.GetHistoryByMonitorAsync(monitor.Id);

        Assert.That(paged.TotalCount, Is.EqualTo(1));
    }

    [Test]
    public async Task GetUptimePercentageAsync_ShouldReturnPercentage()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "MonitorUptime",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true,
            Checks = new List<MonitorCheck>
            {
                new MonitorCheck { CheckedAt = DateTime.UtcNow.AddMinutes(-1), IsSuccess = true },
                new MonitorCheck { CheckedAt = DateTime.UtcNow, IsSuccess = false }
            }
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var dto = await _monitorService.GetUptimePercentageAsync(monitor.Id, DateTime.UtcNow.AddHours(-1),
            DateTime.UtcNow.AddHours(1));

        Assert.That(dto.Percentage, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task GetLatencyGraph_ShouldReturnPoints()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "MonitorLatency",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var check = new MonitorCheck
        {
            Id = Guid.NewGuid(),
            MonitorId = monitor.Id,
            CheckedAt = DateTime.UtcNow,
            IsSuccess = true,
            ResponseTimeMs = 123
        };
        await _context.MonitorChecks.AddAsync(check);
        await _context.SaveChangesAsync();

        var points =
            await _monitorService.GetLatencyGraph(monitor.Id, DateTime.UtcNow.AddHours(-1),
                DateTime.UtcNow.AddHours(1));

        Assert.That(points.Count, Is.EqualTo(1));
        Assert.That(points[0].ResponseTimeMs, Is.EqualTo(123));
    }

    [Test]
    public async Task GetDownTimesAsync_ShouldReturnDowntimes()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "MonitorDownTimes",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var checks = new[]
        {
            new MonitorCheck
            {
                Id = Guid.NewGuid(), MonitorId = monitor.Id, CheckedAt = DateTime.UtcNow.AddMinutes(-2),
                IsSuccess = false
            },
            new MonitorCheck
            {
                Id = Guid.NewGuid(), MonitorId = monitor.Id, CheckedAt = DateTime.UtcNow.AddMinutes(-1),
                IsSuccess = true
            }
        };
        await _context.MonitorChecks.AddRangeAsync(checks);
        await _context.SaveChangesAsync();

        var downtimes = await _monitorService.GetDownTimesAsync(monitor.Id);

        Assert.That(downtimes.Count, Is.EqualTo(1));
        Assert.That(downtimes[0].Start, Is.Not.Null);
        Assert.That(downtimes[0].End, Is.Not.Null);
    }

    [Test]
    public async Task GetMonitorSummaryAsync_ShouldReturnSummary()
    {
        var monitor = new Monitor
        {
            Id = Guid.NewGuid(),
            UserId = _currentUserId,
            Name = "MonitorSummary",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var stat = new MonitorStatistic
        {
            Id = Guid.NewGuid(),
            MonitorId = monitor.Id,
            PeriodEnd = DateTime.UtcNow,
            UptimePercent = 99,
            AvgResponseTimeMs = 100,
            TotalChecks = 10,
            FailedChecks = 1,
            IncidentsCount = 1
        };
        await _context.MonitorStatistics.AddAsync(stat);
        await _context.SaveChangesAsync();

        var summary = await _monitorService.GetMonitorSummaryAsync(monitor.Id, 1);

        Assert.That(summary.UptimePercent, Is.EqualTo(99));
        Assert.That(summary.AvgResponseTimeMs, Is.EqualTo(100));
        Assert.That(summary.TotalChecks, Is.EqualTo(10));
        Assert.That(summary.FailedChecks, Is.EqualTo(1));
        Assert.That(summary.IncidentsCount, Is.EqualTo(1));
    }

    private class HttpMessageHandlerStub : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}