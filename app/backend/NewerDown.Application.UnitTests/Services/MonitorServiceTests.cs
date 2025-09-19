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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.Request;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class MonitorServiceTests
{
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<IUserContextService> _userContextServiceMock;
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<IScopedTimeProvider> _timeProviderMock;
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<IUserService> _userServiceMock;
    
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
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);
        _userServiceMock = new();

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
            _timeProviderMock.Object,
            _userManagerMock.Object,
            _userServiceMock.Object);
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
        // Arrange
        var dto = new AddMonitorDto
        {
            Name = "Monitor1",
            Target = "https://example.com",
            Type = MonitorType.Http,
            IntervalSeconds = 60,
            IsActive = true
        };

        // Act
        var result = await _monitorService.CreateMonitorAsync(dto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(await _context.Monitors.AnyAsync(m => m.Name == "Monitor1"), Is.True);
    }

    [Test]
    public async Task GetMonitorByIdAsync_ShouldReturnMonitor()
    {
        // Arrange
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

        var request = new GetByIdDto()
        {
            Id = monitor.Id
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _monitorService.GetMonitorByIdAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Name, Is.EqualTo(monitor.Name));
    }

    [Test]
    public async Task UpdateMonitorAsync_ShouldUpdateMonitor()
    {
        // Arrange
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

        // Act
        var result = await _monitorService.UpdateMonitorAsync(monitor.Id, dto);
        var updated = await _context.Monitors.FindAsync(monitor.Id);
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(updated.Name, Is.EqualTo("Monitor3Updated"));
        Assert.That(updated.Target, Is.EqualTo("https://updated.com"));
        Assert.That(updated.IsActive, Is.False);
    }

    [Test]
    public async Task DeleteMonitorAsync_ShouldDeleteMonitor()
    {
        // Arrange
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

        var request = new DeleteMonitorDto()
        {
            Id = monitor.Id
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _monitorService.DeleteMonitorAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(await _context.Monitors.FindAsync(monitor.Id), Is.Null);
    }

    [Test]
    public async Task PauseMonitorAsync_ShouldPauseMonitor()
    {
        // Arrange
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

        var request = new GetByIdDto()
        {
            Id = monitor.Id
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _monitorService.PauseMonitorAsync(request);
        var paused = await _context.Monitors.FindAsync(monitor.Id);
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(paused.IsActive, Is.False);
    }

    [Test]
    public async Task ResumeMonitorAsync_ShouldResumeMonitor()
    {
        // Arrange
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
        
        var request = new GetByIdDto()
        {
            Id = monitor.Id
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _monitorService.ResumeMonitorAsync(request);
        var resumed = await _context.Monitors.FindAsync(monitor.Id);
        
        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(resumed.IsActive, Is.True);
    }

    [Test]
    public async Task GetAllMonitors_ShouldReturnMonitors()
    {
        // Arrange
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

        var user = new User()
        {
            Id = _currentUserId
        };

        var roles = new List<string> { RoleType.Administrator.ToString() };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();
        
        _userServiceMock.Setup(userService => userService.GetUserByIdAsync(_currentUserId))
            .ReturnsAsync(user);

        _userManagerMock.Setup(userManager => userManager.GetRolesAsync(user))
            .ReturnsAsync(roles);

        // Act
        var result = await _monitorService.GetAllMonitorsAsync();

        // Assert
        Assert.That(result.Any(m => m.Name == monitor.Name), Is.True);
    }

    [Test]
    public async Task ExportMonitorCsvAsync_ShouldReturnCsvBytes()
    {
        // Arrange
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
        
        var request = new GetByIdDto()
        {
            Id = monitor.Id
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        // Act
        var bytes = await _monitorService.ExportMonitorCsvAsync(request);

        // Assert
        Assert.That(bytes.Length, Is.GreaterThan(0));
    }

    [Test]
    public async Task ImportMonitorsFromCsvAsync_ShouldImportMonitors()
    {
        // Arrange
        var csv = "Name,Url,Type,CheckIntervalSeconds,IsActive\nMonitorCsvImport,https://import.com,Http,60,true";
        var fileMock = new Mock<Microsoft.AspNetCore.Http.IFormFile>();
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

        // Act
        await _monitorService.ImportMonitorsFromCsvAsync(fileMock.Object);

        // Assert
        Assert.That(await _context.Monitors.AnyAsync(m => m.Name == "MonitorCsvImport"), Is.True);
    }

    [Test]
    public async Task GetMonitorStatusAsync_ShouldReturnUpOrDown()
    {
        // Arrange
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
                new() { CheckedAt = DateTime.UtcNow, IsSuccess = true }
            }
        };
        
        var request = new GetByIdDto()
        {
            Id = monitor.Id
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        // Act
        var status = await _monitorService.GetMonitorStatusAsync(request);

        // Assert
        Assert.That(status, Is.EqualTo(MonitorStatus.Up));
    }

    [Test]
    public async Task GetHistoryByMonitorAsync_ShouldReturnPagedChecks()
    {
        // Arrange
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

        var check = new MonitorCheck
        {
            Id = Guid.NewGuid(),
            MonitorId = monitor.Id,
            CheckedAt = DateTime.UtcNow,
            IsSuccess = true
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.MonitorChecks.AddAsync(check);
        await _context.SaveChangesAsync();

        // Act
        var paged = await _monitorService.GetHistoryByMonitorAsync(monitor.Id);

        // Assert
        Assert.That(paged.TotalCount, Is.EqualTo(1));
    }

    [Test]
    public async Task GetUptimePercentageAsync_ShouldReturnPercentage()
    {
        // Arrange
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
                new() { CheckedAt = DateTime.UtcNow.AddMinutes(-1), IsSuccess = true },
                new() { CheckedAt = DateTime.UtcNow, IsSuccess = false }
            }
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        // Act
        var response = await _monitorService.GetUptimePercentageAsync(monitor.Id, DateTime.UtcNow.AddHours(-1),
            DateTime.UtcNow.AddHours(1));

        // Assert
        Assert.That(response.Percentage, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task GetLatencyGraph_ShouldReturnPoints()
    {
        // Arrange
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

        var check = new MonitorCheck
        {
            Id = Guid.NewGuid(),
            MonitorId = monitor.Id,
            CheckedAt = DateTime.UtcNow,
            IsSuccess = true,
            ResponseTimeMs = 123
        };
        
        await _context.Monitors.AddAsync(monitor);
        await _context.MonitorChecks.AddAsync(check);
        await _context.SaveChangesAsync();

        // Act
        var points = await _monitorService.GetLatencyGraph(monitor.Id, DateTime.UtcNow.AddHours(-1),
                DateTime.UtcNow.AddHours(1));

        // Assert
        Assert.That(points.Count, Is.EqualTo(1));
        Assert.That(points[0].ResponseTimeMs, Is.EqualTo(123));
    }

    [Test]
    public async Task GetDownTimesAsync_ShouldReturnDowntimes()
    {
        // Arrange
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
        
        var request = new GetByIdDto()
        {
            Id = monitor.Id
        };

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
        
        await _context.Monitors.AddAsync(monitor);
        await _context.MonitorChecks.AddRangeAsync(checks);
        await _context.SaveChangesAsync();

        // Act
        var downtimes = await _monitorService.GetDownTimesAsync(request);

        // Assert
        Assert.That(downtimes.Count, Is.EqualTo(1));
        Assert.That(downtimes[0].Start, Is.Not.Null);
        Assert.That(downtimes[0].End, Is.Not.Null);
    }

    [Test]
    public async Task GetMonitorSummaryAsync_ShouldReturnSummary()
    {
        // Arrange
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

        var statistic = new MonitorStatistic
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
        
        await _context.Monitors.AddAsync(monitor);
        await _context.MonitorStatistics.AddAsync(statistic);
        await _context.SaveChangesAsync();

        // Act
        var summary = await _monitorService.GetMonitorSummaryAsync(monitor.Id, 1);

        // Assert
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