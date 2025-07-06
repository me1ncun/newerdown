using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NewerDown.Application.Services;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    
    private ApplicationDbContext _context;
    private UserService userService;
    
    private readonly Guid currentUserId = Guid.Parse("0a600fd2-cd43-4f95-b0c4-5e531288c19e");
    
    [SetUp]
    public void Setup()
    {
        _httpContextAccessorMock = new();
        
        userService = new UserService(_httpContextAccessorMock.Object, _context);
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
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
            new Claim("userId", currentUserId.ToString())
        };
        
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = principal });
        
        // Act
        var result = userService.GetUserId();
        
        // Assert
        Assert.That(result, Is.EqualTo(currentUserId));
    }
}