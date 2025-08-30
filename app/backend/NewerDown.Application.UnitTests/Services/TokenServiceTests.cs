using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using NewerDown.Application.Services;
using NewerDown.Domain.Entities;

[TestFixture]
public class TokenServiceTests
{
    private IConfiguration _configuration;
    private TokenService _tokenService;

    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"JwtKey", "supersecurekeyfortesting1234567890"},
            {"JWT:ValidIssuer", "TestIssuer"},
            {"JWT:ValidAudience", "TestAudience"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        
        _tokenService = new TokenService(_configuration);
    }

    [Test]
    public void GenerateAccessToken_ValidUser_ReturnsToken()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        // Act
        var token = _tokenService.GenerateAccessToken(claims);

        // Assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public void GenerateAccessToken_TokenContainsCorrectClaims()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = _tokenService.GenerateAccessToken(claims);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.That(jwt.Claims.First(c => c.Type == "unique_name").Value, Is.EqualTo(user.UserName));
        Assert.That(jwt.Claims.First(c => c.Type == "email").Value, Is.EqualTo(user.Email));
    }

    [Test]
    public void GenerateAccessToken_HasValidExpiration()
    {
        var claims = new[] { new Claim("dummy", "value") };

        var token = _tokenService.GenerateAccessToken(claims);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.That(jwt.ValidTo, Is.GreaterThan(DateTime.UtcNow));
        Assert.That(jwt.ValidTo, Is.LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(15.1)));
    }

    [Test]
    public void GenerateAccessToken_UsesHmacSha256Signature()
    {
        var claims = new[] { new Claim("dummy", "value") };

        var token = _tokenService.GenerateAccessToken(claims);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.That(jwt.Header.Alg, Is.EqualTo("HS256"));
    }
}
