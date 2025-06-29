using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class NotificationRuleServiceTests
{
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<IUserService> _userServiceMock;
    
    private ApplicationDbContext _context;
    private NotificationRuleService _notificationRuleService;
    
    private readonly Guid currentUserId = Guid.Parse("0a600fd2-cd43-4f95-b0c4-5e531288c19e");
    
    [SetUp]
    public void Setup()
    {
        _cacheServiceMock = new();
        _userServiceMock = new();
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(NotificationRuleMappingProfile));
        }).CreateMapper();

        _userServiceMock.Setup(x => x.GetUserId()).Returns(currentUserId);

        _notificationRuleService = new NotificationRuleService(
            _context,
            mapper,
            _cacheServiceMock.Object,
            _userServiceMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
    
    [Test]
    public async Task GetNotificationRuleByIdAsync_ShouldReturnNotificationRule_WhenExists()
    {
        // Arrange
        var notificationRule = new NotificationRule
        {
            Id = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            UserId = currentUserId
        };
        
        await _context.NotificationRules.AddAsync(notificationRule);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _notificationRuleService.GetNotificationRuleByIdAsync(notificationRule.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(notificationRule.Id, Is.EqualTo(result.Id));
    }
    
    [Test]
    public void GetNotificationRuleByIdAsync_ShouldThrowException_WhenNotExists()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _notificationRuleService.GetNotificationRuleByIdAsync(nonExistentId));

        Assert.That(ex.Message, Is.EqualTo("NotificationRule was not found."));
    }


    [Test]
    public async Task GetAllAsync_ShouldReturnAllNotificationRule_WhenExists()
    {
        // Arrange
        var notificationRules = new List<NotificationRule>
        {
            new NotificationRule
            {
                Id = Guid.NewGuid(),
                ServiceId = Guid.NewGuid(),
                UserId = currentUserId
            },
            new NotificationRule
            {
                Id = Guid.NewGuid(),
                ServiceId = Guid.NewGuid(),
                UserId = currentUserId
            }
        };
        
        await _context.NotificationRules.AddRangeAsync(notificationRules);
        await _context.SaveChangesAsync();
        
        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<NotificationRuleDto>>(It.IsAny<string>()))
            .ReturnsAsync((IEnumerable<NotificationRuleDto>?)null);

        // Act
        var result = await _notificationRuleService.GetAllAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(notificationRules.Count));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllNotificationRule_WhenExistsInCache()
    {
        // Arrange
        var notificationRules = new List<NotificationRuleDto>
        {
            new NotificationRuleDto
            {
                Id = Guid.NewGuid(),
                ServiceId = Guid.NewGuid(),
                UserId = currentUserId
            },
            new NotificationRuleDto
            {
                Id = Guid.NewGuid(),
                ServiceId = Guid.NewGuid(),
                UserId = currentUserId
            }
        };
        
        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<NotificationRuleDto>>(It.IsAny<string>()))
            .ReturnsAsync(notificationRules.AsEnumerable());

        // Act
        var result = await _notificationRuleService.GetAllAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(notificationRules.Count));
    }

    [Test]
    public async Task CreateNotificationRuleAsync_ShouldAddNotificationRule_WhenServiceRuleExists()
    {
        // Arrange
        var existingRule = new NotificationRule
        {
            Id = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            UserId = currentUserId,
            Channel = NotificationChannel.Email,
            Target = "existing@example.com",
            NotifyOnFailure = true,
            NotifyOnRecovery = false
        };

        await _context.NotificationRules.AddAsync(existingRule);
        await _context.SaveChangesAsync();

        var dto = new AddNotificationRuleDto
        {
            ServiceId = existingRule.ServiceId,
            Channel = NotificationChannel.PushNotification,
            Target = "new@example.com",
            NotifyOnFailure = false,
            NotifyOnRecovery = true
        };

        // Act
        await _notificationRuleService.CreateNotificationRuleAsync(dto);

        // Assert
        var rules = await _context.NotificationRules
            .Where(x => x.UserId == currentUserId && x.ServiceId == dto.ServiceId)
            .ToListAsync();

        Assert.That(rules.Count, Is.EqualTo(2));
        Assert.That(rules.Any(r => r.Target == "new@example.com"));
    }
    
    [Test]
    public async Task DeleteNotificationRuleAsync_ShouldRemoveNotificationRule_WhenExists()
    {
        // Arrange
        var rule = new NotificationRule
        {
            Id = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            UserId = currentUserId,
            Channel = NotificationChannel.Email,
            Target = "delete@example.com",
            NotifyOnFailure = true,
            NotifyOnRecovery = true
        };

        await _context.NotificationRules.AddAsync(rule);
        await _context.SaveChangesAsync();

        // Act
        await _notificationRuleService.DeleteNotificationRuleAsync(rule.Id);

        // Assert
        var deleted = await _context.NotificationRules.FirstOrDefaultAsync(x => x.Id == rule.Id);
        Assert.That(deleted, Is.Null);
    }
}