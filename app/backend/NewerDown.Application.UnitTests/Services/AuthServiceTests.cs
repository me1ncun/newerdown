using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using NewerDown.Application.Services;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.UnitTests.Services;

[TestFixture]
public class AuthServiceTests
{
    private IConfiguration _сonfiguration;
    
    private AuthService _authService;
    
    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"JwtKey", "supersecurekeyfortesting1234567890"}
        };

        _сonfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        
        _authService = new AuthService(_сonfiguration);
    }

   [Test]
    public void GenerateToken_ValidUser_ReturnsToken()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };

        // Act
        var token = _authService.GenerateToken(user);

        // Assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public void GenerateToken_TokenContainsCorrectClaims()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };

        // Act
        var token = _authService.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.That(user.UserName, Is.EqualTo(jwt.Claims.First(c => c.Type == "username").Value));
        Assert.That(user.Email, Is.EqualTo(jwt.Claims.First(c => c.Type == "email").Value));
        Assert.That(user.Id.ToString(), Is.EqualTo(jwt.Claims.First(c => c.Type == "userId").Value));
    }

    [Test]
    public void GenerateToken_HasValidExpiration()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };

        var token = _authService.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        
        Assert.That(jwt.ValidTo, Is.GreaterThan(DateTime.UtcNow));
        Assert.That(jwt.ValidTo, Is.LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(15.1)));
    }

    [Test]
    public void GenerateToken_UsesHmacSha256Signature()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };

        var token = _authService.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.That("HS256", Is.EqualTo(jwt.Header.Alg));
    }
}