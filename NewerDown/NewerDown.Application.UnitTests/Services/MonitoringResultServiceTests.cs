using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Domain.Entities;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class MonitoringResultServiceTests
{
    private ApplicationDbContext _context;
    private MonitoringResultService _monitoringResultService;
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MonitoringResultMappingProfile));
        }).CreateMapper();

        _monitoringResultService = new MonitoringResultService(
            _context,
            mapper);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
    
    [Test]
    public async Task GetMonitoringResultsAsync_ShouldReturnPagedResults_WhenFilterIsApplied()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var results = new List<MonitoringResult>
        {
            new MonitoringResult { Id = Guid.NewGuid(), ServiceId = serviceId, CheckedAt = DateTime.UtcNow, Error = "Test error 1" },
            new MonitoringResult { Id = Guid.NewGuid(), ServiceId = serviceId, CheckedAt = DateTime.UtcNow.AddMinutes(-10), Error = "Test error 2" }
        };
        
        await _context.MonitoringResults.AddRangeAsync(results);
        await _context.SaveChangesAsync();

        // Act
        var response = await _monitoringResultService.GetMonitoringResultsAsync(serviceId.ToString(), 1, 10);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Items.Count(), Is.EqualTo(2));
    }
    
    [Test]
    public async Task GetMonitoringResultsByDaysAsync_ShouldReturnResults_WhenDaysFilterIsApplied()
    {
        // Arrange
        var results = new List<MonitoringResult>
        {
            new MonitoringResult { Id = Guid.NewGuid(), ServiceId = Guid.NewGuid(), CheckedAt = DateTime.UtcNow.AddDays(-1), Error = "Test error 1" },
            new MonitoringResult { Id = Guid.NewGuid(), ServiceId = Guid.NewGuid(), CheckedAt = DateTime.UtcNow.AddDays(-5), Error = "Test error 2" }
        };
        
        await _context.MonitoringResults.AddRangeAsync(results);
        await _context.SaveChangesAsync();

        // Act
        var response = await _monitoringResultService.GetMonitoringResultsByDaysAsync(3);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count(), Is.EqualTo(1));
    }
}