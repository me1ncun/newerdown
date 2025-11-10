using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.Services;
using NewerDown.Application.UnitTests.Helpers;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class UserContextServiceTests
{
    private Mock<IMapper> _mapperMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<Lazy<IUserPhotoProvider>> _userPhotoProviderMock;
    
    private ApplicationDbContext _context;
    private UserContextService _userContextService;
    
    private readonly Guid _currentUserId = Guid.NewGuid();
    
    [SetUp]
    public void Setup()
    {
        _httpContextAccessorMock = new();
        _mapperMock = new ();
        _userPhotoProviderMock = new();
        
        _context = new DbContextProvider().BuildDbContext();
        _context.Database.EnsureCreated();
        
        _userContextService = new UserContextService(_context, _httpContextAccessorMock.Object, _mapperMock.Object, _userPhotoProviderMock.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
    
    [Test]
    public void GetUserId_AuthenticatedUser_ReturnsUserId()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, _currentUserId.ToString())
        };
        
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = principal });
        
        // Act
        var result = _userContextService.GetUserId();
        
        // Assert
        Assert.That(result, Is.EqualTo(_currentUserId));
    }
}