/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class MonitorServiceTests
{
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<IUserService> _userServiceMock;
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    
    private ApplicationDbContext _context;
    private MonitorService _monitorService;
    
    private readonly Guid currentUserId = Guid.Parse("0a600fd2-cd43-4f95-b0c4-5e531288c19e");
    
    [SetUp]
    public void Setup()
    {
        _cacheServiceMock = new();
        _userServiceMock = new();
        _httpClientFactoryMock = new();
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MonitorMappingProfile));
        }).CreateMapper();

        _userServiceMock.Setup(x => x.GetUserId()).Returns(currentUserId);

        _monitorService = new MonitorService(
            _context,
            mapper,
            _cacheServiceMock.Object,
            _userServiceMock.Object,
            _httpClientFactoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
    
    [Test]
    public async Task GetServiceByIdAsync_ShouldReturnService_WhenExists()
    {
        // Arrange
        var service = new Monitor()
        {
            Id = Guid.NewGuid(),
            UserId = currentUserId,
            Name = "Test Service",
            Url = "https://example.com",
        };
        
        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _monitorService.GetMonitorByIdAsync(service.Id);
        
        Assert.That(service.Id, Is.EqualTo(result.Id));
    }
    
    [Test]
    public void GetServiceByIdAsync_ShouldThrowException_WhenNotExists()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _monitorService.GetMonitorByIdAsync(nonExistentId));

        Assert.That(ex.Message, Is.EqualTo("Service was not found."));
    }
}*/