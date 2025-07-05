using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class ServicesServiceTests
{
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<IUserService> _userServiceMock;
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    
    private ApplicationDbContext _context;
    private ServicesService _servicesService;
    
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
            cfg.AddProfile(typeof(ServicesMappingProfile));
        }).CreateMapper();

        _userServiceMock.Setup(x => x.GetUserId()).Returns(currentUserId);

        _servicesService = new ServicesService(
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
        var service = new Service()
        {
            Id = Guid.NewGuid(),
            UserId = currentUserId,
            Name = "Test Service",
            Url = "https://example.com",
        };
        
        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _servicesService.GetServiceByIdAsync(service.Id);
        
        Assert.That(service.Id, Is.EqualTo(result.Id));
    }
    
    [Test]
    public void GetServiceByIdAsync_ShouldThrowException_WhenNotExists()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _servicesService.GetServiceByIdAsync(nonExistentId));

        Assert.That(ex.Message, Is.EqualTo("Service was not found."));
    }
}