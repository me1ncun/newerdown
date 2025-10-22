using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Application.UnitTests.Helpers;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserContextService> _userContextServiceMock;
    
    private UserService _userService;
    private ApplicationDbContext _context;
    private IMapper _mapper;
    
    private readonly Guid _currentUserId = Guid.NewGuid();

    [SetUp]
    public void Setup()
    {
        _userContextServiceMock = new();
        
        _context = new DbContextProvider().BuildDbContext();
        _context.Database.EnsureCreated();
        
        _mapper = new MapperConfiguration(cfg => { 
            cfg.AddProfiles(new List<Profile>
            {
                new UserMappingProfile()
            });
        }).CreateMapper();
        
        _userService = new UserService(
            _userContextServiceMock.Object,
            _context,
            _mapper
            );
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Id = _currentUserId,
            UserName = "John Doe",
            Email = "john@example.com"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUserByIdAsync(_currentUserId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(user.Id, Is.EqualTo(result!.Id));
    }

    [Test]
    public async Task GetAllUsersAsync_ShouldReturnMappedUserDtos()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), UserName = "Alice", Email = "alice@example.com" },
            new User { Id = Guid.NewGuid(), UserName = "Bob", Email = "bob@example.com" }
        };

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.That(result, Has.Count.EqualTo(users.Count));
        Assert.That(result.Select(x => x.Email), Is.EquivalentTo(users.Select(x => x.Email)));
    }

    [Test]
    public async Task UpdateUserAsync_ShouldUpdateAndReturnUserDto_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Id = _currentUserId,
            UserName = "OldName",
            Email = "user@example.com"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        _userContextServiceMock.Setup(x => x.GetUserId()).Returns(_currentUserId);

        var updateDto = new UpdateUserDto
        {
            UserName = "NewName"
        };

        // Act
        var result = await _userService.UpdateUserAsync(updateDto);

        // Assert
        Assert.That(result.IsSuccess);
        Assert.That("NewName", Is.EqualTo(result.Value.UserName));
    }

    [Test]
    public async Task UpdateUserAsync_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        _userContextServiceMock.Setup(x => x.GetUserId()).Returns(_currentUserId);

        var updateDto = new UpdateUserDto
        {
            UserName = "Name"
        };

        // Act
        var result = await _userService.UpdateUserAsync(updateDto);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That("Users.UserNotFound", Is.EqualTo(result.Error.Code));
    }

    [Test]
    public async Task DeleteUserAsync_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Id = _currentUserId,
            UserName = "ToDelete",
            Email = "delete@example.com"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        _userContextServiceMock.Setup(x => x.GetUserId()).Returns(_currentUserId);

        // Act
        var result = await _userService.DeleteUserAsync();

        // Assert
        Assert.That(result.IsSuccess);
        Assert.That(_context.Users.Any(x => x.Id == _currentUserId), Is.False);
    }

    [Test]
    public async Task DeleteUserAsync_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        _userContextServiceMock.Setup(x => x.GetUserId()).Returns(_currentUserId);

        // Act
        var result = await _userService.DeleteUserAsync();

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That("Users.UserNotFound", Is.EqualTo(result.Error?.Code));
    }
}