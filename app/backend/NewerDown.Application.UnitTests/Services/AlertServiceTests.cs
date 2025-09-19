using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class AlertServiceTests
{
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<IUserContextService> _userContextServiceMock;
    
    private ApplicationDbContext _context;
    private AlertService _alertService;
    private IScopedTimeProvider _scopedTimeProvider;
    
    private readonly Guid _currentUserId = Guid.Parse("0a600fd2-cd43-4f95-b0c4-5e531288c19e");
    
    [SetUp]
    public void Setup()
    {
        _cacheServiceMock = new();
        _userContextServiceMock = new();
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _scopedTimeProvider = new ScopedTimeProvider(TimeProvider.System);
        _context.Database.EnsureCreated();
        
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(AlertMappingProfile));
        }).CreateMapper();

        _userContextServiceMock.Setup(x => x.GetUserId()).Returns(_currentUserId);

        _alertService = new AlertService(
            _context,
            mapper,
            _cacheServiceMock.Object,
            _userContextServiceMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
    
    [Test]
    public async Task GetAlertByIdAsync_ShouldReturnAlert_WhenExists()
    {
        // Arrange
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            MonitorId = Guid.NewGuid(),
            Type = AlertType.Email,
            Target = "email@email.com",
            CreatedAt = _scopedTimeProvider.UtcNow(),
            UserId = _currentUserId
        };
        
        await _context.Alerts.AddAsync(alert);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _alertService.GetAlertByIdAsync(alert.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(alert.Id, Is.EqualTo(result.Id));
    }
    
    [Test]
    public void GetAlertByIdAsync_ShouldThrowException_WhenNotExists()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _alertService.GetAlertByIdAsync(nonExistentId));

        Assert.That(ex.Message, Is.EqualTo("Alert was not found."));
    }


    [Test]
    public async Task GetAllAsync_ShouldReturnAllAlerts_WhenExists()
    {
        // Arrange
        var alerts = new List<Alert>
        {
            new()
            {
                Id = Guid.NewGuid(),
                MonitorId = Guid.NewGuid(),
                Type = AlertType.Email,
                Target = "email@email.com",
                CreatedAt = _scopedTimeProvider.UtcNow(),
                UserId = _currentUserId
            },
            new()
            {
                Id = Guid.NewGuid(),
                MonitorId = Guid.NewGuid(),
                Type = AlertType.Email,
                Target = "email@email.com",
                CreatedAt = _scopedTimeProvider.UtcNow(),
                UserId = _currentUserId
            }
        };
        
        await _context.Alerts.AddRangeAsync(alerts);
        await _context.SaveChangesAsync();
        
        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<AlertDto>>(It.IsAny<string>()))
            .ReturnsAsync((IEnumerable<AlertDto>?)null);

        // Act
        var result = await _alertService.GetAllAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(alerts.Count));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllAlerts_WhenExistsInCache()
    {
        // Arrange
        var alerts = new List<AlertDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                MonitorId = Guid.NewGuid(),
                Type = AlertType.Email,
                Target = "email@email.com",
                UserId = _currentUserId
            },
            new()
            {
                Id = Guid.NewGuid(),
                MonitorId = Guid.NewGuid(),
                Type = AlertType.Email,
                Target = "email@email.com",
                UserId = _currentUserId
            }
        };
        
        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<AlertDto>>(It.IsAny<string>()))
            .ReturnsAsync(alerts.AsEnumerable());

        // Act
        var result = (await _alertService.GetAllAsync()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(alerts.Count));
    }

    [Test]
    public async Task CreateAlertAsync_ShouldAddAlert_WhenMonitorExists()
    {
        // Arrange
        var monitor = new Monitor()
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Type = MonitorType.Http,
            Target = "https://example.com",
            UserId = _currentUserId,
            IntervalSeconds = 30,
            CreatedAt = _scopedTimeProvider.UtcNow(),
            IsActive = true
        };

        await _context.Monitors.AddAsync(monitor);
        await _context.SaveChangesAsync();

        var alert = new AddAlertDto
        {
            Type = AlertType.Email,
            MonitorId = monitor.Id,
            Target = "new@example.com"
        };

        // Act
        await _alertService.CreateAlertAsync(alert);

        // Assert
        var alerts = await _context.Alerts.FirstOrDefaultAsync(x => x.UserId == _currentUserId && x.MonitorId == alert.MonitorId);

        Assert.That(alerts, Is.Not.Null);
        Assert.That(alerts.Target == "new@example.com");
    }
    
    [Test]
    public async Task DeleteAlertAsync_ShouldRemoveAlert_WhenExists()
    {
        // Arrange
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            MonitorId = Guid.NewGuid(),
            Type = AlertType.Email,
            Target = "email@email.com",
            CreatedAt = _scopedTimeProvider.UtcNow(),
            UserId = _currentUserId
        };

        await _context.Alerts.AddAsync(alert);
        await _context.SaveChangesAsync();

        // Act
        await _alertService.DeleteAlertAsync(alert.Id);

        // Assert
        var deleted = await _context.Alerts.FirstOrDefaultAsync(x => x.Id == alert.Id);
        Assert.That(deleted, Is.Null);
    }
}