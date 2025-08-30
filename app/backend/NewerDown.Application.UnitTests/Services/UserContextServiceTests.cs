using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using NewerDown.Application.Services;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class UserContextServiceTests
{
    private Mock<IMapper> _mapperMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<IUserService> _userServiceMock;
    
    private UserContextService userContextService;
    
    private readonly Guid currentUserId = Guid.Parse("0a600fd2-cd43-4f95-b0c4-5e531288c19e");
    
    [SetUp]
    public void Setup()
    {
        _httpContextAccessorMock = new();
        _mapperMock = new ();
        _userServiceMock = new();
        
        userContextService = new UserContextService(_httpContextAccessorMock.Object, _mapperMock.Object, _userServiceMock.Object);
    }
    
    [Test]
    public void GetUserId_AuthenticatedUser_ReturnsUserId()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim("userId", currentUserId.ToString())
        };
        
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = principal });
        
        // Act
        var result = userContextService.GetUserId();
        
        // Assert
        Assert.That(result, Is.EqualTo(currentUserId));
    }
}